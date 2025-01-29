using Microsoft.EntityFrameworkCore;
using OBSSystem.Core.Entities;

namespace OBSSystem.Infrastructure.Configurations
{
    public class OBSContext : DbContext
    {
        public OBSContext(DbContextOptions<OBSContext> options) : base(options) { }

        // DbSet Tanımları
        public DbSet<User> Users { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Grade> Grades { get; set; }
        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<Announcement> Announcements { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Enrollment - Student ilişkisinde cascade yerine restrict/no action kullanıyoruz
            modelBuilder.Entity<Enrollment>()
                .HasOne(e => e.Student)
                .WithMany(s => s.Enrollments)
                .HasForeignKey(e => e.StudentID)
                .OnDelete(DeleteBehavior.Restrict); // Cascade yerine Restrict kullanıldı

            // Enrollment - Course ilişkisinde cascade yerine restrict/no action kullanıyoruz
            modelBuilder.Entity<Enrollment>()
                .HasOne(e => e.Course)
                .WithMany(c => c.Enrollments)
                .HasForeignKey(e => e.CourseID)
                .OnDelete(DeleteBehavior.Restrict); // Cascade yerine Restrict kullanıldı

            // User ve türetilmiş sınıflar (TPH - Table Per Hierarchy)
            modelBuilder.Entity<User>()
       .HasDiscriminator<string>("Role")
       .HasValue<User>("User")
       .HasValue<Student>("Student")
       .HasValue<Teacher>("Teacher")
       .HasValue<Admin>("Admin");


            // Primary Key Tanımları
            modelBuilder.Entity<User>().HasKey(u => u.UserID);

            // Teacher-Course İlişkisi
            modelBuilder.Entity<Course>()
                .HasOne(c => c.Teacher)
                .WithMany(t => t.Courses)
                .HasForeignKey(c => c.TeacherID)
                .OnDelete(DeleteBehavior.Cascade); // Bu kalsın

            // Attendance İlişkileri (Cascade Delete Kaldırıldı)
            modelBuilder.Entity<Attendance>()
                .HasOne(a => a.Student)
                .WithMany(s => s.Attendances)
                .HasForeignKey(a => a.StudentID)
                .OnDelete(DeleteBehavior.Restrict); // Cascade yerine Restrict kullanıldı

            modelBuilder.Entity<Attendance>()
                .HasOne(a => a.Course)
                .WithMany()
                .HasForeignKey(a => a.CourseID)
                .OnDelete(DeleteBehavior.Cascade);

            // Grade İlişkileri
            modelBuilder.Entity<Grade>()
                .HasOne(g => g.Student)
                .WithMany(s => s.Grades)
                .HasForeignKey(g => g.StudentID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Grade>()
                .HasOne(g => g.Course)
                .WithMany()
                .HasForeignKey(g => g.CourseID)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Grade>()
                .Property(g => g.Score)
                .IsRequired(); // Score alanını zorunlu hale getirme

            // Öğrenci-Bölüm İlişkisi
            modelBuilder.Entity<Student>()
                .HasOne(s => s.Department)
                .WithMany(d => d.Students)
                .HasForeignKey(s => s.DepartmentID)
                .OnDelete(DeleteBehavior.Restrict); // Bölüm silindiğinde öğrenciler etkilenmez


            modelBuilder.Entity<RefreshToken>()
    .HasOne(rt => rt.User)
    .WithMany()
    .HasForeignKey(rt => rt.UserId)
    .OnDelete(DeleteBehavior.Cascade);


        }

    }
}
