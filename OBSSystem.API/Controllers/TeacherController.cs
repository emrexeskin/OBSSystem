using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace OBSSystem.API.Controllers
{
    [Authorize(Roles = "Teacher")] // Sadece Teacher rolüne izin veriyoruz
    [Route("api/[controller]")]
    [ApiController]
    public class TeacherController : ControllerBase
    {
        [HttpGet("manage-courses")]
        public IActionResult ManageCourses()
        {
            return Ok(new { message = "Teacher can manage courses!" });
        }
    }
}
