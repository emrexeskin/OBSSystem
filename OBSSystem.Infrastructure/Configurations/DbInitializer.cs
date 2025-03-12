using Microsoft.EntityFrameworkCore;
using OBSSystem.Core.Entities;
using OBSSystem.Infrastructure.Helpers;
using System.Linq;

namespace OBSSystem.Infrastructure.Configurations
{
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
                            Password = passwordHasher.HashPassword("admin123"),
                            Role = "Admin" // Artık string olarak saklıyoruz
                        },
                        new User
                        {
                            Name = "Teacher User",
                            Email = "teacher@obs.com",
                            Password = passwordHasher.HashPassword("teacher123"),
                            Role = "Teacher" // Artık string olarak saklıyoruz
                        },
                        new User
                        {
                            Name = "Student User",
                            Email = "student@obs.com",
                            Password = passwordHasher.HashPassword("student123"),
                            Role = "Student" // Artık string olarak saklıyoruz
                        }
                    );
                    context.SaveChanges();
                }

                // Dersler ekleniyor (Course tablosu)
                if (!context.Courses.Any())
                {
                    var teacher =
                        context.Users.FirstOrDefault(u => u.Role == "Teacher"); // Artık string olarak karşılaştırıyoruz

                    context.Courses.AddRange(
                        new Course
                        {
                            CourseName = "Introduction to Programming",
                            Schedule = "Sunday 9:00 12.12.1212",
                            TeacherID = teacher.UserID // Öğretmen ataması yapılıyor
                        },
                        new Course
                        {
                            CourseName = "Physics I",
                            Schedule = "Sunday 9:00 12.12.1212",
                            TeacherID = teacher.UserID // Öğretmen ataması yapılıyor
                        }
                    );
                    context.SaveChanges();
                }

                // Öğrencilerin derslere kayıt edilmesi (Enrollment tablosu)
                if (!context.Enrollments.Any())
                {
                    var student =
                        context.Users.FirstOrDefault(u => u.Role == "Student"); // Artık string olarak karşılaştırıyoruz
                    var courses = context.Courses.ToList();

                    context.Enrollments.AddRange(
                        new Enrollment
                        {
                            StudentID = student.UserID,
                            CourseID = courses.First().CourseID // İlk dersi seçiyoruz
                        },
                        new Enrollment
                        {
                            StudentID = student.UserID,
                            CourseID = courses.Last().CourseID // Son dersi seçiyoruz
                        }
                    );
                    context.SaveChanges();
                }

                // ActivityLog Ekleyelim
                if (!context.ActivityLogs.Any())
                {
                    var admin = context.Users.FirstOrDefault(u => u.Role == "Admin");

                    context.ActivityLogs.AddRange(
                        new ActivityLog
                        {
                            UserID = admin.UserID,
                            Action = "System Initialization",
                            Description = "System was initialized with default users and courses.",
                            Timestamp = DateTime.UtcNow,
                            IpAddress = "127.0.0.1" // İp adresi burada örnek olarak ekleniyor
                        }
                    );
                    context.SaveChanges();
                }
            }
        }
    }
}
