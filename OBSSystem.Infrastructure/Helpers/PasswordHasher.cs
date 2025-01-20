using BCrypt.Net;

namespace OBSSystem.Infrastructure.Helpers
{
    public static class PasswordHasher
    {
        // Şifreyi hashlemek için kullanılır
        public static string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        // Şifreyi hashlenmiş şifreyle karşılaştırır
        public static bool VerifyPassword(string hashedPassword, string password)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }
    }
}
