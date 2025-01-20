using OBSSystem.Application.Exceptions;
using OBSSystem.Application.Interfaces;
using OBSSystem.Application.Validators;
using OBSSystem.Core.Entities;

namespace OBSSystem.Application.Services
{
    public class UserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHasher _passwordHasher;

        public UserService(IUserRepository userRepository, IPasswordHasher passwordHasher)
        {
            _userRepository = userRepository;
            _passwordHasher = passwordHasher;
        }

        public IEnumerable<User> GetAllUsers()
        {
            return _userRepository.GetAllUsers();
        }

        public User GetUserById(int id)
        {
            var user = _userRepository.GetUserById(id);
            if (user == null)
                throw new UserNotFoundException($"User with ID {id} not found.");
            return user;
        }

        public void CreateUser(User user)
        {
            if (_userRepository.IsEmailTaken(user.Email))
                throw new EmailAlreadyTakenException($"Email '{user.Email}' is already in use.");

            // Şifre politikası doğrulaması
            var passwordErrors = PasswordPolicyValidator.ValidatePassword(user.Password);
            if (passwordErrors.Count > 0)
                throw new PasswordPolicyException(passwordErrors);

            // Şifreyi hashle
            user.Password = _passwordHasher.HashPassword(user.Password);

            // Kullanıcıyı oluştur
            _userRepository.CreateUser(user);
        }

        public void UpdateUser(int id, User updatedUser)
        {
            var existingUser = _userRepository.GetUserById(id);
            if (existingUser == null)
                throw new UserNotFoundException($"User with ID {id} not found.");

            if (_userRepository.IsEmailTaken(updatedUser.Email, id))
                throw new EmailAlreadyTakenException($"Email '{updatedUser.Email}' is already in use.");

            // Şifre değişikliği varsa politikayı kontrol et
            if (!_passwordHasher.VerifyPassword(existingUser.Password, updatedUser.Password))
            {
                var passwordErrors = PasswordPolicyValidator.ValidatePassword(updatedUser.Password);
                if (passwordErrors.Count > 0)
                    throw new PasswordPolicyException(passwordErrors);

                updatedUser.Password = _passwordHasher.HashPassword(updatedUser.Password);
            }

            // Diğer alanları güncelle
            existingUser.Name = updatedUser.Name;
            existingUser.Email = updatedUser.Email;
            existingUser.Role = updatedUser.Role;

            _userRepository.UpdateUser(existingUser);
        }

        public void DeleteUser(int id)
        {
            var existingUser = _userRepository.GetUserById(id);
            if (existingUser == null)
                throw new UserNotFoundException($"User with ID {id} not found.");

            _userRepository.DeleteUser(id);
        }
    }
}
