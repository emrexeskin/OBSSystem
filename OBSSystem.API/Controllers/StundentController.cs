using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace OBSSystem.API.Controllers
{
    [Authorize(Roles = "Student")] // Sadece Student rolüne izin veriyoruz
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        [HttpGet("view-grades")]
        public IActionResult ViewGrades()
        {
            return Ok(new { message = "Student can view grades!" });
        }


    }
}
