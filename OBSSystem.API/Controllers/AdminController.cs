using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace OBSSystem.API.Controllers
{
    [Authorize(Roles = "Admin")] // Sadece Admin rolüne izin veriyoruz
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        [HttpGet("manage-users")]
        public IActionResult ManageUsers()
        {
            return Ok(new { message = "Admin users can manage the system!" });
        }
    }
}