using System.Collections.Generic;
using OBSSystem.Core.Entities;

namespace OBSSystem.Application.Interfaces
{
    public interface IUserRepository
    {
        IEnumerable<User> GetAllUsers();
        IEnumerable<Teacher> GetAllTeachers();
        IEnumerable<Student> GetAllStudents();
        IEnumerable<Teacher> GetAllTeachersWithCourses();
        User GetUserByEmail(string email);
        User GetUserById(int id);
        void CreateUser(User user);
        void UpdateUser(User user);
        void DeleteUser(int id);
        bool IsEmailTaken(string email, int? excludeUserId = null);
    }
}