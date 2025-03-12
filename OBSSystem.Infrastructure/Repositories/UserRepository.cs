using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using OBSSystem.Application.Interfaces;
using OBSSystem.Core.Entities;
using OBSSystem.Infrastructure.Configurations;

namespace OBSSystem.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly OBSContext _context;

        public UserRepository(OBSContext context)
        {
            _context = context;
        }

        // Tüm kullanıcıları senkron getir
        public IEnumerable<User> GetAllUsers()
        {
            return _context.Users.ToList();
        }

        // Tüm öğretmenleri senkron getir
        public IEnumerable<Teacher> GetAllTeachers()
        {
            return _context.Teachers.ToList();
        }

        // Dersleriyle birlikte öğretmenleri getir (senkron)
        public IEnumerable<Teacher> GetAllTeachersWithCourses()
        {
            return _context.Teachers
                .Include(t => t.Courses)
                .ToList();
        }

        // Tüm öğrencileri senkron getir
        public IEnumerable<Student> GetAllStudents()
        {
            return _context.Students.ToList();
        }

        // E-posta ile kullanıcıyı getir (senkron)
        public User GetUserByEmail(string email)
        {
            return _context.Users.SingleOrDefault(u => u.Email == email);
        }

        // ID ile kullanıcıyı getir (senkron)
        public User GetUserById(int id)
        {
            return _context.Users
                .Include(u => (u as Teacher).Courses)
                .SingleOrDefault(u => u.UserID == id);
        }

        // Kullanıcı oluştur (senkron)
        public void CreateUser(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        // Kullanıcıyı güncelle (senkron)
        public void UpdateUser(User user)
        {
            _context.Users.Update(user);
            _context.SaveChanges();
        }

        // Kullanıcıyı sil (senkron)
        public void DeleteUser(int id)
        {
            var user = _context.Users.Find(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
            }
        }

        // E-posta kullanımda mı kontrol et (senkron)
        public bool IsEmailTaken(string email, int? excludeUserId = null)
        {
            return _context.Users.Any(u => u.Email == email && u.UserID != excludeUserId);
        }
    }
}
