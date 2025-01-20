using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OBSSystem.Core.Entities;
using OBSSystem.Infrastructure.Configurations;
using OBSSystem.Infrastructure.Helpers;

namespace OBSSystem.API.Controllers
{
    [Authorize(Roles = "Admin")] // Sadece Admin yetkisi olanlar bu endpointlere erişebilir
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly OBSContext _context;

        public UserController(OBSContext context)
        {
            _context = context;
        }

        // Kullanıcıları listeleme
        [HttpGet]
        public IActionResult GetAllUsers()
        {
            var users = _context.Users.ToList();
            return Ok(users);
        }

        // Kullanıcı detaylarını görüntüleme
        [HttpGet("{id}")]
        public IActionResult GetUserById(int id)
        {
            var user = _context.Users.Find(id);
            if (user == null)
            {
                return NotFound(new { message = "User not found" });
            }

            return Ok(user);
        }

        // Yeni kullanıcı ekleme
        [HttpPost]
        public IActionResult CreateUser([FromBody] User newUser)
        {
            if (newUser == null)
            {
                return BadRequest(new { message = "Invalid user data" });
            }

            // Aynı email ile bir kullanıcı zaten var mı?
            if (_context.Users.Any(u => u.Email == newUser.Email))
            {
                return Conflict(new { message = "A user with the same email already exists." });
            }

            // Şifreyi hashleyerek kaydet
            newUser.Password = PasswordHasher.HashPassword(newUser.Password);


            _context.Users.Add(newUser);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetUserById), new { id = newUser.UserID }, newUser);
        }

        // Kullanıcı güncelleme
        [HttpPut("{id}")]
        public IActionResult UpdateUser(int id, [FromBody] User updatedUser)
        {
            var user = _context.Users.Find(id);
            if (user == null)
            {
                return NotFound(new { message = "User not found" });
            }

            // Email alanını güncellerken aynı emailden başka kullanıcı var mı kontrolü
            if (_context.Users.Any(u => u.Email == updatedUser.Email && u.UserID != id))
            {
                return Conflict(new { message = "A user with the same email already exists." });
            }

            user.Name = updatedUser.Name;
            user.Email = updatedUser.Email;

            // Şifre hashlenmiş olarak güncellenir
            user.Password = PasswordHasher.HashPassword(updatedUser.Password);


            user.Role = updatedUser.Role;

            _context.SaveChanges();

            return Ok(user);
        }

        // Kullanıcı silme
        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            var user = _context.Users.Find(id);
            if (user == null)
            {
                return NotFound(new { message = "User not found" });
            }

            _context.Users.Remove(user);
            _context.SaveChanges();

            return NoContent();
        }
    }
}
