using AutoMapper;
using ChatHubProject.Application.Dto;
using ChatHubProject.Application.Infrastructure;
using ChatHubProject.Application.Model;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ChatHubProject.Webapi.Controllers
{
    public class UserController : EntityReadController<User>
    {
        private readonly IConfiguration _config;
        private readonly bool _isDevelopment;

        public UserController(ChatHubContext db, IMapper mapper, IConfiguration config, IHostEnvironment _env) : base(db, mapper)
        {
            _config = config;
            _isDevelopment = _env.IsDevelopment();
        }

        /// <summary>
        /// GET /api/user/
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Administration")]
        [HttpGet]
        public async Task<IActionResult> GetAllUser() => await GetAll<UserDto>();

        /// <summary>
        /// GET /api/user/guid
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        [Authorize(Roles = "Administration")]
        [HttpGet("{guid}")]
        public async Task<IActionResult> GetUser(Guid guid)
        {
            return await GetByGuid(guid, a => new
            {
                a.Guid,
                a.Username,
                a.Password,
                a.Email,
                a.Role,
                a.Group,
            });
        }

        /// <summary>
        /// POST /api/user/loginspg
        /// Login using either student or self-made account
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
                var claims = new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Username.ToString()),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, role)
                };
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    Expires = DateTime.UtcNow + lifetime,
                    SigningCredentials = new SigningCredentials(
                        new SymmetricSecurityKey(secret),
                        SecurityAlgorithms.HmacSha256Signature)

                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                var cookie = new ClaimsIdentity
                (
                    claims,
                    Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults.AuthenticationScheme
                );
                var authProperties = new AuthenticationProperties
                {
                    AllowRefresh = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddHours(3),
                };
                await HttpContext.SignInAsync
                (
                    Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(cookie),
                    authProperties
                );
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
                    user = new User(credentials.Username, credentials.Password, $"{credentials.Username}@spengergasse.at", Userrole.Pupil.ToString());
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
                var claims = new Claim[]
                {
                    new Claim(ClaimTypes.Name, currentUser.Cn),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, role),
                    new Claim("Group", group)
                };
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    // Payload for our JWT.
                    Subject = new ClaimsIdentity(claims),
                    Expires = DateTime.UtcNow + lifetime,
                    SigningCredentials = new SigningCredentials(
                        new SymmetricSecurityKey(secret),
                        SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                var cookie = new ClaimsIdentity
                (
                    claims,
                    Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults.AuthenticationScheme
                );
                var authProperties = new AuthenticationProperties
                {
                    AllowRefresh = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddHours(3),
                };
                await HttpContext.SignInAsync
                (
                    Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(cookie),
                    authProperties
                );
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
                user = new User(credentials.Username, credentials.Password, credentials.Email, Userrole.Pupil.ToString());
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

        /// <summary>
        /// We cannot access the cookie in JavaScript. To check the auth state, we can send a request
        /// to /api/user/userinfo. So we can set our application state.
        /// </summary>
        [Authorize]
        [HttpGet("userinfo")]
        public IActionResult GetUserInfo()
        {
            var authenticated = HttpContext.User.Identity?.IsAuthenticated ?? false;
            if (!authenticated) { return Unauthorized(); }
            return Ok(new
            {
                Username = HttpContext.User.Identity?.Name,
                IsAdmin = HttpContext.User.IsInRole("Administration"),
            });
        }

        [HttpGet("logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return NoContent();
        }
    }
}