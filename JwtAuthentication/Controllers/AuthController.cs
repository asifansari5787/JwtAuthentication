using JwtAuthentication.Data;
using JwtAuthentication.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JwtAuthentication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        //public static User user = new User();
        private readonly IConfiguration _configuration;
        private readonly DataContext _context;

        public AuthController(IConfiguration configuration, DataContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        [HttpPost("register")]
        public ActionResult<User> Register(UserDto request)
        {
            if(_context.Users.Any(u => u.Username == request.Username))
            {
                return BadRequest("User already exists");
            }

            string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            var user = new User()
            {
                Username = request.Username,
                PasswordHash = passwordHash,
            };

            //user.Username = request.Username;
            //user.PasswordHash = passwordHash;
            _context.Users.Add(user);
            _context.SaveChanges();

            return Ok("User successfully created");
        }

        [HttpPost("login")]
        public ActionResult<User> Login(UserDto request)
        {
            var user = _context.Users.FirstOrDefault(u => u.Username == request.Username);
            if(user == null)
            {
                return BadRequest("User not found");
            }

            //if(user.Username != request.Username)
            //{
            //    return BadRequest("User not found");
            //}

            if(!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                return BadRequest("wrong password");
            }

            var token = CreateToken(user);
            return Ok(token);

        }

        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                  _configuration.GetSection("AppSettings:Token").Value!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var token = new JwtSecurityToken(
                   claims: claims,
                   expires: DateTime.Now.AddDays(1),
                   signingCredentials: creds
                );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }
    }
}
