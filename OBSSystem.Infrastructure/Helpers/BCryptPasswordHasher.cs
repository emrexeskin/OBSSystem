using BCrypt.Net;
using OBSSystem.Application.Interfaces;

namespace OBSSystem.Infrastructure.Helpers
{
    public class BCryptPasswordHasher : IPasswordHasher
    {
        // Şifreyi hashler
        public string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        // Hashlenmiş şifreyi doğrular
        public bool VerifyPassword(string hashedPassword, string password)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }

        // Şifrenin güçlü olup olmadığını kontrol eder
        public bool IsPasswordStrong(string password)
        {
            return password.Length >= 8 &&
                   password.Any(char.IsUpper) &&
                   password.Any(char.IsLower) &&
                   password.Any(char.IsDigit) &&
                   password.Any(ch => !char.IsLetterOrDigit(ch));
        }
    }
}
