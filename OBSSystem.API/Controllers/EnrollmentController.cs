using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OBSSystem.Application.Services;
using OBSSystem.Core.Entities;
using System.Security.Claims;

namespace OBSSystem.API.Controllers
{
    [Authorize(Roles = "Student")]
    [Route("api/[controller]")]
    [ApiController]
    public class EnrollmentController : ControllerBase
    {
        private readonly EnrollmentService _enrollmentService;

        public EnrollmentController(EnrollmentService enrollmentService)
        {
            _enrollmentService = enrollmentService;
        }

        [HttpPost]
        public IActionResult AddEnrollment([FromBody] int courseId)
        {
            try
            {
                var studentId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                _enrollmentService.AddEnrollment(studentId, courseId);
                return Ok(new { message = "Enrollment added successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet]
        public IActionResult GetEnrollments()
        {
            try
            {
                var studentId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                var enrollments = _enrollmentService.GetEnrollmentsByStudent(studentId);
                return Ok(enrollments);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public IActionResult RemoveEnrollment(int id)
        {
            try
            {
                _enrollmentService.RemoveEnrollment(id);
                return Ok(new { message = "Enrollment removed successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
