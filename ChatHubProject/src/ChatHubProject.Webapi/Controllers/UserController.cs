﻿using AutoMapper;
using ChatHubProject.Application.Dto;
using ChatHubProject.Application.Infrastructure;
using ChatHubProject.Application.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ChatHubProject.Webapi.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    [AllowAnonymous]
    public class UserController : ControllerBase
    {
        public record LoginDto(string Username, string Password);
        public record RegisterDto(string Username, string Password, string Email);

        private readonly IConfiguration _config;
        private readonly bool _isDevelopment;
        private readonly ChatHubContext _db;
        private readonly IMapper _mapper;

        public UserController(IConfiguration config, IHostEnvironment _env, ChatHubContext db, IMapper mapper)
        {
            _config = config;
            _isDevelopment = _env.IsDevelopment();
            _db = db;
            _mapper = mapper;
        }

        /// <summary>
        /// GET /api/user
        /// List all users.
        /// Only for users which has the role admin in the claim of the JWT.
        /// </summary>
        [Authorize(Roles = "Administration")]
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var user = await _db.Users
                .Select(a => new
                {
                    a.Guid,
                    a.Username,
                    a.Email,
                    a.Role,
                    a.Group,
                })
                .ToListAsync();
            if (user is null) { return BadRequest(); }
            return Ok(user);
        }

        /// <summary>
        /// GET /api/user/guid
        /// List one users.
        /// Only for users which has the role admin in the claim of the JWT.
        /// </summary>
        [Authorize(Roles = "Admin")]
        [HttpGet("{guid:Guid}")]
        public async Task<IActionResult> GetOneUser(Guid guid) 
        {
            var user = await _db.Users
                .Where(a => a.Guid == guid)
                .Select(a => new
                {
                    a.Guid,
                    a.Username,
                    a.Email,
                    a.Role,
                    a.Group,
                })
                .FirstOrDefaultAsync(a => a.Guid == guid);
            if (user is null) { return NotFound(); }
            return Ok(user);
        }

        /// <summary>
        /// POST /api/user/loginspg
        /// Login using student account
        /// </summary>
        /// <param name="credentials"></param>
        /// <returns></returns>
        [HttpPost("login")]
        public async Task<IActionResult> LoginSPG([FromBody] LoginDto credentials)
        {
            var secret = Convert.FromBase64String(_config["Secret"]);
            var lifetime = TimeSpan.FromHours(3);
            var searchuser = _config["Searchuser"];
            var searchpass = _config["Searchpass"];
            var localAdmins = _config["LocalAdmins"].Split(",");

            var user = await _db.Users.FirstOrDefaultAsync(a => a.Username == credentials.Username);
            if (user is not null) 
            {
                if (!user.CheckPassword(credentials.Password)) { return Unauthorized(); }

                var role = Userrole.Pupil.ToString();
                var tokenHandler = new JwtSecurityTokenHandler();
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.Name, user.Username.ToString()),
                        new Claim(ClaimsIdentity.DefaultRoleClaimType, role)
                    }),
                    Expires = DateTime.UtcNow + lifetime,
                    SigningCredentials = new SigningCredentials(
                        new SymmetricSecurityKey(secret),
                        SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                return Ok(new
                {
                    user.Username,
                    Role = role,
                    UserGuid = user.Guid,
                    user.Group,
                    Token = tokenHandler.WriteToken(token)
                });
            }
            else
            {
                using var service = _isDevelopment && !string.IsNullOrEmpty(searchuser)
                    ? AdService.Login(searchuser, searchpass, credentials.Username)
                    : AdService.Login(credentials.Username, credentials.Password);
                var currentUser = service.CurrentUser;
                if (currentUser is null) { return Unauthorized(); }

                if (user is null)
                {
                    user = new User(credentials.Username, credentials.Password, $"{credentials.Username}@spengergasse.at", Userrole.Pupil);
                    await _db.Users.AddAsync(user);
                    try { await _db.SaveChangesAsync(); }
                    catch (DbUpdateException) { return BadRequest(); }
                }
                if (!user.CheckPassword(credentials.Password)) { return Unauthorized(); }

                var role = localAdmins.Contains(currentUser.Cn)
                                ? AdUserRole.Management.ToString()
                                : currentUser.Role.ToString();
                var group = (currentUser.Role, currentUser.Classes.Length > 0) switch
                {
                    (AdUserRole.Pupil, true) => currentUser.Classes[0],
                    (AdUserRole.Pupil, false) => "Unknown class",
                    (AdUserRole.Teacher, _) => AdUserRole.Teacher.ToString(),
                    (AdUserRole.Management, _) => AdUserRole.Teacher.ToString(),
                    (_, _) => AdUserRole.Administration.ToString()
                };
                var tokenHandler = new JwtSecurityTokenHandler();
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    // Payload for our JWT.
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.Name, currentUser.Cn),
                        new Claim(ClaimsIdentity.DefaultRoleClaimType, role),
                        new Claim("Group", group)
                    }),
                    Expires = DateTime.UtcNow + lifetime,
                    SigningCredentials = new SigningCredentials(
                        new SymmetricSecurityKey(secret),
                        SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                return Ok(new
                {
                    Username = currentUser.Cn,
                    UserGuid = user.Guid,
                    Role = role,
                    Group = group,
                    Token = tokenHandler.WriteToken(token)
                });
            }
        }

        /// <summary>
        /// POST /api/user/register
        /// </summary>
        /// <param name="credentials"></param>
        /// <returns></returns>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto credentials)
        {
            var user = await _db.Users.FirstOrDefaultAsync(a => a.Email == credentials.Email);
            if (user is null)
            {
                user = new User(credentials.Username, credentials.Password, credentials.Email, Userrole.Pupil);
                await _db.Users.AddAsync(user);
                try { await _db.SaveChangesAsync(); }
                catch (DbUpdateException) { return BadRequest(); }
            }
            else { return BadRequest("User is already in the database."); }
            if (!user.CheckPassword(credentials.Password)) { return Unauthorized(); }
            return Ok(new
            {
                user.Username,
                user.Guid,
                user.Email,
                user.Role,
            });
        }

        /// <summary>
        /// DELETE Request /api/user/guid with JSON body
        /// Deletes a user in the database.
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        [HttpDelete("{guid:Guid}")]
        public async Task<IActionResult> DeleteUser(Guid guid)
        {
            var users = await _db.Users.FirstOrDefaultAsync(a => a.Guid == guid);
            if (users is null) { return NotFound(); }
            _db.Users.Remove(users);
            try { await _db.SaveChangesAsync(); }
            catch (DbUpdateException) { return BadRequest(); }
            return NoContent();
        }

        /// <summary>
        /// PUT Request /api/user/guid with JSON body
        /// Updates a user in the database. 
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="userDto"></param>
        /// <returns></returns>
        [HttpPut("{guid:Guid}")]
        public async Task<IActionResult> EditUser(Guid guid, UserDto userDto)
        {
            if (guid != userDto.Guid) { return BadRequest(); }
            var user = await _db.Users.FirstOrDefaultAsync(a => a.Guid == guid);
            if (user is null) { return NotFound(); }
            _mapper.Map(userDto, user);
            try { await _db.SaveChangesAsync(); }
            catch (DbUpdateException) { return BadRequest(); }
            return NoContent();
        }
    }
}