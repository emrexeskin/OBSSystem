using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OBSSystem.Application.Exceptions;
using OBSSystem.Application.Services;
using OBSSystem.Core.Entities;

namespace OBSSystem.API.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public IActionResult GetAllUsers()
        {
            var users = _userService.GetAllUsers();
            return Ok(users);
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
