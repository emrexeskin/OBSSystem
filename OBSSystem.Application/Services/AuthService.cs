using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using OBSSystem.Application.Interfaces;
using OBSSystem.Core.Entities;

namespace OBSSystem.Application.Services
{
    public class AuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;

        public AuthService(IUserRepository userRepository, IPasswordHasher passwordHasher)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
        }

        public string Authenticate(string email, string password, string secretKey, string issuer, string audience, int tokenExpiryMinutes)
        {
            var user = _userRepository.GetUserByEmail(email);
            if (user == null || !_passwordHasher.VerifyPassword(user.Password, password))
            {
                throw new UnauthorizedAccessException("Invalid credentials.");
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserID.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            };

            if (user is Teacher teacher)
            {
                claims.Add(new Claim("role", "Teacher"));
                claims.Add(new Claim("teacherId", teacher.UserID.ToString()));
            }
            else if (user is Student student)
            {
                claims.Add(new Claim("role", "Student"));
                claims.Add(new Claim("studentId", student.UserID.ToString()));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(tokenExpiryMinutes),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
