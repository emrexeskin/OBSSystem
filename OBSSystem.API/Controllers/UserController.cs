using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OBSSystem.Application.Exceptions;
using OBSSystem.Application.Services;
using OBSSystem.Core.Entities;
using System.Security.Claims;   

namespace OBSSystem.API.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }
        
        
        [AllowAnonymous]
        [HttpGet("me")]
        public IActionResult GetMe()
        {
            try
            {
                // Kullanıcının ID'sini al
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                
                // Kullanıcıyı al
                var user = _userService.GetUserById(userId);
                
                if (user == null)
                {
                    return NotFound(new { message = "User not found." });
                }

                // Kullanıcı bilgilerini döndür
                return Ok(new
                {
                    user.UserID,
                    user.Name,
                    user.Email,
                    user.Role
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet]
        public IActionResult GetAllUsers()
        {
            var users = _userService.GetAllUsers();
            return Ok(users);
        }

        [HttpGet("teachers")]
        public IActionResult GetAllTeachers([FromQuery] bool includeCourses = false)
        {
            try
            {
                var teachers = _userService.GetAllTeachers(includeCourses);

                if (teachers == null || !teachers.Any())
                {
                    return NotFound(new { message = "No teachers found." });
                }

                return Ok(teachers);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred.", details = ex.Message });
            }
        }

 
        [HttpGet("students")]
        public IActionResult GetAllStudents()
        {
            try
            {
                var students = _userService.GetAllStudents();
                return Ok(students);
            }
            catch (StudentNotFoundException ex)
            {   
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "An unexpected error occurred.", details = ex.Message });
            }
        }
        
        [AllowAnonymous] //Herkes erişebilir
        [HttpGet("strict-teachers")]
        public IActionResult GetAllTeachersRestrict()
        {
            try
            {
                var teachers = _userService.GetAllTeachersRestrict();
                return Ok(teachers);
            }
            catch (TeacherNotFoundException ex)
            {   
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = "An unexpected error occurred.", details = ex.Message });
            }
        }

        
        [HttpGet("{id}")]
        public IActionResult GetUserById(int id)
        {
            try
            {
                var user = _userService.GetUserById(id);
                return Ok(user);
            }
            catch (UserNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpPost]
        public IActionResult CreateUser([FromBody] User newUser)
        {
            try
            {
                _userService.CreateUser(newUser);
                return CreatedAtAction(nameof(GetUserById), new { id = newUser.UserID }, newUser);
            }
            catch (PasswordPolicyException ex)
            {
                return BadRequest(new { message = ex.Message, errors = ex.Errors });
            }
            catch (EmailAlreadyTakenException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

    }
}
