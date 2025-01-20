using OBSSystem.Core.Entities;

namespace OBSSystem.Application.Interfaces
{
    public interface IUserRepository
    {
        IEnumerable<User> GetAllUsers();
        User GetUserById(int id);
        void CreateUser(User user);
        void UpdateUser(User user);
        void DeleteUser(int id);
        bool IsEmailTaken(string email, int? excludeUserId = null);
    }
}
