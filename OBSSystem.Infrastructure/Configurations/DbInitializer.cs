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

            // Bölüm ekleme
            if (!context.Departments.Any())
            {
                context.Departments.AddRange(
                    new Department { DepartmentName = "Computer Engineering", Faculty = "Engineering" },
                    new Department { DepartmentName = "Physics", Faculty = "Science" },
                    new Department { DepartmentName = "Mathematics", Faculty = "Science" }
                );
                context.SaveChanges();
            }

            // Şifre hashlemek için BCryptPasswordHasher örneği oluştur
            var passwordHasher = new BCryptPasswordHasher();

            // Eğer User tablosunda veri yoksa test verileri ekle
            if (!context.Users.Any())
            {
                context.Users.AddRange(
                    new User
                    {
                        Name = "Admin User",
                        Email = "admin@obs.com",
                        Password = passwordHasher.HashPassword("admin123"), // Şifre hashleniyor
                        Role = "Admin"
                    },
                    new User
                    {
                        Name = "Teacher User",
                        Email = "teacher@obs.com",
                        Password = passwordHasher.HashPassword("teacher123"), // Şifre hashleniyor
                        Role = "Teacher"
                    },
                    new User
                    {
                        Name = "Student User",
                        Email = "student@obs.com",
                        Password = passwordHasher.HashPassword("student123"), // Şifre hashleniyor
                        Role = "Student"
                    }
                );
                context.SaveChanges();
            }
        }
    }
}
