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
        public record CredentialsDto(string username, string password, string email);

        private readonly IConfiguration _config;
        private readonly bool _isDevelopment;
        private readonly ChatHubContext _db;

        public UserController(IConfiguration config, IHostEnvironment _env, ChatHubContext db)
        {
            _config = config;
            _isDevelopment = _env.IsDevelopment();
            _db = db;
        }

        /// <summary>
        /// POST /api/user/loginms
        /// </summary>
        /// <param name="credentials"></param>
        /// <returns></returns>
        [HttpPost("loginspg")]
        public async Task<IActionResult> LoginSPG([FromBody] CredentialsDto credentials)
        {
            var lifetime = TimeSpan.FromHours(3);
            var searchuser = _config["Searchuser"];
            var searchpass = _config["Searchpass"];
            var secret = Convert.FromBase64String(_config["Secret"]);
            var localAdmins = _config["LocalAdmins"].Split(",");

            using var service = _isDevelopment && !string.IsNullOrEmpty(searchuser)
                ? AdService.Login(searchuser, searchpass, credentials.username)
                : AdService.Login(credentials.username, credentials.password);
            var currentUser = service.CurrentUser;
            if (currentUser is null) { return Unauthorized(); }

            var user = await _db.Users.FirstOrDefaultAsync(a => a.Username == credentials.username);
            if (user is null)
            {
                user = new User(credentials.username, credentials.password, credentials.email, Userrole.User);
                await _db.Users.AddAsync(user);
                try { await _db.SaveChangesAsync(); }
                catch (DbUpdateException) { return BadRequest(); }
            }
            if (!user.CheckPassword(credentials.password)) { return Unauthorized(); }

            var role = localAdmins.Contains(currentUser.Cn)
                            ? AdUserRole.Management.ToString() : currentUser.Role.ToString();
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

        /// <summary>
        /// POST /api/user/login
        /// </summary>
        /// <param name="credentials"></param>
        /// <returns></returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] CredentialsDto credentials)
        {
            var secret = Convert.FromBase64String(_config["Secret"]);
            var lifetime = TimeSpan.FromHours(3);

            var user = await _db.Users.FirstOrDefaultAsync(a => a.Username == credentials.username);
            if (user is null) { return Unauthorized(); }
            if (!user.CheckPassword(credentials.password)) { return Unauthorized(); }

            var role = Userrole.User.ToString();
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
                Token = tokenHandler.WriteToken(token)
            });
        }

        /// <summary>
        /// POST /api/user/register
        /// </summary>
        /// <param name="credentials"></param>
        /// <returns></returns>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] CredentialsDto credentials)
        {
            var user = await _db.Users.FirstOrDefaultAsync(a => a.Username == credentials.username);
            if (user is null)
            {
                user = new User(credentials.username, credentials.password, credentials.email, Userrole.User);
                await _db.Users.AddAsync(user);
                try { await _db.SaveChangesAsync(); }
                catch (DbUpdateException) { return BadRequest(); }
            }
            else { return BadRequest("User is already in the database."); }
            if (!user.CheckPassword(credentials.password)) { return Unauthorized(); }
            return Ok(new
            {
                user.Username,
                user.Guid,
                user.Email,
                user.Role,
            });
        }
    }
}