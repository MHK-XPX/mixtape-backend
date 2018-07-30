using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

using Mixtape.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.Azure.KeyVault.Models;

namespace mixtape.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly DataContext _context;
        private AuthSetting _authSettings;

        public AuthController(DataContext context, IOptions<AuthSetting> authSettings)
        {
            _context = context;
            _authSettings = authSettings.Value;
        }

        private string CreateToken(User user)
        {
            //string secret = _authSettings.SECRET; //DEV
            string secret = Environment.GetEnvironmentVariable("SECRET"); //PROD
            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
            var claims = new Claim[] { new Claim(JwtRegisteredClaimNames.Sub, user.UserId.ToString()) };
            var jwt = new JwtSecurityToken(claims: claims, signingCredentials: signingCredentials);
            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }

        private OkObjectResult AuthResult(User user)
        {
            string token = CreateToken(user);

            return Ok(
                new
                {
                    Username = user.Username,
                    Firstname = user.FirstName,
                    Lastname = user.LastName,
                    Token = token
                }
                );
        }
        /// <summary>
        /// Get the current authenticated user
        /// </summary>
        /// <returns>Current authenticated user</returns>
        /// <response code="200">User</response>
        /// <response code="400">Error model</response>
        [Authorize]
        [HttpGet("me")]
        public IActionResult Get()
        {
            string id = HttpContext.User.Claims.First().Value;
            User _user = _context.User.SingleOrDefault(u => u.UserId.ToString() == id);

            _user.Password = null;

            return Ok(_user);
        }


        /// <summary>
        /// Attempt to login
        /// </summary>
        /// <returns>Authenticated User</returns>
        /// <response code="200">Authenticated User</response>
        /// <response code="400">Error model</response>
        [HttpPost("login")]
        public IActionResult Login([FromBody]LoginUser user)
        {
            string username = user.Username;
            string password = user.Password;

            if(string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                return Unauthorized();
            }

            User _user = _context.User.SingleOrDefault(x => x.Username == username);

            if(_user == null)
                return Unauthorized(); //return BadRequest("Unable to find username");

            var passwordHasher = new PasswordHasher<User>();
           //if (passwordHasher.VerifyHashedPassword(_user, _user.Password, password) == 0)
           //     return Unauthorized(); //return BadRequest("Invalid password");
           if(!String.Equals(password, _user.Password))
            {
                return Unauthorized();
            }

            return AuthResult(_user);
        }


        private class AuthUser
        {
            public string Username { get; set; }
            public string Firstname { get; set; }
            public string Lastname { get; set; }
            public string Token { get; set; }
        }

        public class LoginUser
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }
    }
}
