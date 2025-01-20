using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OBSSystem.Core.Entities;
using OBSSystem.Infrastructure.Configurations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using OBSSystem.Infrastructure.Helpers; // Doğru PasswordHasher sınıfı için


namespace OBSSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly OBSContext _context;

        public AuthController(IConfiguration configuration, OBSContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        [HttpPost("login")]
        public IActionResult Login(LoginRequest request)
        {
            // Veritabanından kullanıcıyı email ile kontrol et
            var user = _context.Users.SingleOrDefault(u => u.Email == request.Email);

            if (user == null || !PasswordHasher.VerifyPassword(user.Password, request.Password))

            {
                return Unauthorized(new { message = "Invalid credentials" });
            }

            // Kullanıcı doğrulandıysa JWT oluştur
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(ClaimTypes.Role, user.Role) // Role veritabanından alınır
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
        }
    }

    // LoginRequest modeli
    public class LoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
