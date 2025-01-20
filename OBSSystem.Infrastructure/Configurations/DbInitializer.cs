using Microsoft.EntityFrameworkCore;
using OBSSystem.Core.Entities;
using OBSSystem.Infrastructure.Helpers;

namespace OBSSystem.Infrastructure.Configurations
{
    public static class DbInitializer
    {
        public static void Seed(OBSContext context)
        {
            // Eğer veri tabanı oluşturulmadıysa, migration'ları çalıştır
            context.Database.Migrate();

            // Eğer User tablosunda veri yoksa test verileri ekle
            if (!context.Users.Any())
            {
                context.Users.AddRange(
                    new User
                    {
                        Name = "Admin User",
                        Email = "admin@obs.com",
                        Password = PasswordHasher.HashPassword("admin123"),
                        Role = "Admin"
                    },
                    new User
                    {
                        Name = "Teacher User",
                        Email = "teacher@obs.com",
                        Password = PasswordHasher.HashPassword("teacher123"),
                        Role = "Teacher"
                    },
                    new User
                    {
                        Name = "Student User",
                        Email = "student@obs.com",
                        Password = PasswordHasher.HashPassword("student123"),
                        Role = "Student"
                    }
                );
                context.SaveChanges();
            }
        }
    }
}
