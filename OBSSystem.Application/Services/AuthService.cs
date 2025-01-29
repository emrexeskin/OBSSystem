using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using OBSSystem.Application.Interfaces;
using OBSSystem.Core.Configuration;
using OBSSystem.Core.Entities;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;

namespace OBSSystem.Application.Services
{
    public class AuthService
    {
        private readonly ILogger<AuthService> _logger;
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly HashSet<string> _blacklistedTokens = new();
        private readonly JwtConfig _jwtConfig;

        public AuthService(
            ILogger<AuthService> logger,
            IUserRepository userRepository,
            IPasswordHasher passwordHasher,
            IRefreshTokenRepository refreshTokenRepository,
            IOptionsSnapshot<JwtConfig> jwtConfig)
          
        {
            _logger = logger;
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
            _refreshTokenRepository = refreshTokenRepository ?? throw new ArgumentNullException(nameof(refreshTokenRepository));
            _jwtConfig = jwtConfig.Value ?? throw new ArgumentNullException(nameof(jwtConfig));  // Options üzerinden al

            _logger.LogInformation("AuthService başarıyla oluşturuldu.");
        }

        public (string accessToken, string refreshToken) Authenticate(string email, string password)
        {
            var user = _userRepository.GetUserByEmail(email);
            if (user == null || !_passwordHasher.VerifyPassword(user.Password, password))
            {
                throw new UnauthorizedAccessException("Invalid credentials.");
            }

            var accessToken = GenerateAccessToken(user.UserID);
            var refreshToken = GenerateRefreshToken(user.UserID);

            return (accessToken, refreshToken);
        }

        public string GenerateAccessToken(int userId)
        {
            
            var user = _userRepository.GetUserById(userId);
            if (user == null) throw new UnauthorizedAccessException("Invalid user.");

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserID.ToString()),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfig.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtConfig.Issuer,
                audience: _jwtConfig.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtConfig.AccessTokenExpiryMinutes),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
          
        }

        public string GenerateRefreshToken(int userId)
        {
            
            var refreshToken = new RefreshToken
            {
                Token = Guid.NewGuid().ToString(),
                UserId = userId,
                ExpiryDate = DateTime.UtcNow.AddDays(_jwtConfig.RefreshTokenExpiryDays),
                IsRevoked = false,
                IsUsed = false
            };

            _refreshTokenRepository.Add(refreshToken);
            _refreshTokenRepository.SaveChanges();

            return refreshToken.Token;

          
        }

        public bool ValidateRefreshToken(string refreshToken, int userId)
        {
            var tokenRecord = _refreshTokenRepository.GetByToken(refreshToken);

            if (tokenRecord == null || tokenRecord.IsRevoked || tokenRecord.ExpiryDate <= DateTime.UtcNow || tokenRecord.UserId != userId)
            {
                return false;
            }

            return true;
        }
       
        public void RevokeRefreshToken(string refreshToken)
        {
            var tokenRecord = _refreshTokenRepository.GetByToken(refreshToken);
            if (tokenRecord != null)
            {
                tokenRecord.IsRevoked = true;
                _refreshTokenRepository.Update(tokenRecord);
                _refreshTokenRepository.SaveChanges();
            }
        }

        public int? GetUserIdFromRefreshToken(string refreshToken)
        {
            var tokenRecord = _refreshTokenRepository.GetByToken(refreshToken);

            if (tokenRecord == null || tokenRecord.IsRevoked || tokenRecord.ExpiryDate <= DateTime.UtcNow)
            {
                return null;
            }

            return tokenRecord.UserId;
        }

        public void BlacklistAccessToken(string token)
        {
            _blacklistedTokens.Add(token);
        }

        public bool IsTokenBlacklisted(string token)
        {
            return _blacklistedTokens.Contains(token);
        }

        public void Logout(string token)
        {
            BlacklistAccessToken(token);
        }
    }
}
