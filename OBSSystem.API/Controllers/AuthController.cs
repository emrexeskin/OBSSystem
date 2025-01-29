using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using OBSSystem.Application.Services;
using OBSSystem.Core.Configuration;
using System;
using System.Security.Claims;

namespace OBSSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;
        private readonly JwtConfig _jwtConfig;

        public AuthController(AuthService authService, IOptions<JwtConfig> jwtConfig)
        {
            _authService = authService;
            _jwtConfig = jwtConfig.Value;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            try
            {
                var (accessToken, refreshToken) = _authService.Authenticate(request.Email, request.Password);

                // Refresh token'ı HTTP-Only Cookie'ye ekleme
                Response.Cookies.Append("refreshToken", refreshToken, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTime.UtcNow.AddDays(_jwtConfig.RefreshTokenExpiryDays)
                });

                return Ok(new { accessToken });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            try
            {
                var token = Request.Headers["Authorization"].ToString()?.Replace("Bearer ", "");
                if (!string.IsNullOrEmpty(token))
                {
                    _authService.BlacklistAccessToken(token);
                }

                if (Request.Cookies.ContainsKey("refreshToken"))
                {
                    var refreshToken = Request.Cookies["refreshToken"];
                    _authService.RevokeRefreshToken(refreshToken);
                    Response.Cookies.Delete("refreshToken");
                }

                return Ok(new { message = "Logout successful." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("refresh-token")]
        public IActionResult RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            if (string.IsNullOrEmpty(refreshToken))
            {
                return Unauthorized(new { message = "Refresh token not found." });
            }

            //Refresh Token doğrulama
            var userId = _authService.GetUserIdFromRefreshToken(refreshToken);
            if (userId == null)
            {
                return Unauthorized(new { message = "Invalid refresh token." });
            }

            //Mevcut Access Token'ı Al ve Kara Listeye Ekle
            var currentAccessToken = Request.Headers["Authorization"].ToString()?.Replace("Bearer ", "");
            if (!string.IsNullOrEmpty(currentAccessToken))
            {
                _authService.BlacklistAccessToken(currentAccessToken);
            }

            //Yeni Access Token oluştur
            var newAccessToken = _authService.GenerateAccessToken(userId.Value);

            return Ok(new { accessToken = newAccessToken });
        }

    }

    public class LoginRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
