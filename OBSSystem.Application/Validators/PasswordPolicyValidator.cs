using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace OBSSystem.Application.Validators
{
    public static class PasswordPolicyValidator
    {
        private static readonly string[] BannedPasswords = { "123456", "password", "qwerty", "admin", "letmein" };

        public static List<string> ValidatePassword(string password)
        {
            var errors = new List<string>();

            // Minimum uzunluk kontrolü
            if (password.Length < 8)
                errors.Add("Password must be at least 8 characters long.");

            // Büyük harf kontrolü
            if (!Regex.IsMatch(password, @"[A-Z]"))
                errors.Add("Password must contain at least one uppercase letter.");

            // Küçük harf kontrolü
            if (!Regex.IsMatch(password, @"[a-z]"))
                errors.Add("Password must contain at least one lowercase letter.");

            // Rakam kontrolü
            if (!Regex.IsMatch(password, @"[0-9]"))
                errors.Add("Password must contain at least one number.");

            // Özel karakter kontrolü
            if (!Regex.IsMatch(password, @"[^\w\d\s:]"))
                errors.Add("Password must contain at least one special character.");

            // Yasaklı şifre kontrolü
            if (BannedPasswords.Contains(password.ToLower()))
                errors.Add("Password is too common or insecure.");

            return errors;
        }
    }
}
