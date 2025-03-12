using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OBSSystem.Application.Services;
using OBSSystem.Core.Entities;
using System.Security.Claims;
using OBSSystem.Application.Interfaces;

namespace OBSSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnrollmentController : ControllerBase
    {
        private readonly EnrollmentService _enrollmentService;
        private readonly ICourseRepository _courseRepository;
        private readonly IEnrollmentRepository _enrollmentRepository;

        public EnrollmentController(EnrollmentService enrollmentService, ICourseRepository courseRepository, IEnrollmentRepository enrollmentRepository)
        {
            _enrollmentService = enrollmentService;
            _courseRepository = courseRepository;  // Inject ICourseRepository
            _enrollmentRepository = enrollmentRepository;  // Inject IEnrollmentRepository
        }


        [HttpPost("self")]
        public IActionResult AddEnrollmentSelf([FromBody] int courseId)
        {
            try
            {
                
                var studentIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(studentIdClaim))
                {
                    return BadRequest(new { message = "User ID is missing from the claim." });
                }

                if (!int.TryParse(studentIdClaim, out var studentId))
                {
                    return BadRequest(new { message = "User ID is not a valid integer." });
                }
                
                // Check if course exists
                var course = _courseRepository.GetCourseById(courseId);
                if (course == null)
                {
                    return BadRequest(new { message = "Course not found." });
                }

                // Check if student is already enrolled in the course
                var existingEnrollment = _enrollmentRepository.GetEnrollmentsByStudent(studentId)
                    .FirstOrDefault(e => e.CourseID == courseId);
                if (existingEnrollment != null)
                {
                    return BadRequest(new { message = "Student is already enrolled in this course." });
                }

                // Enroll the student
                _enrollmentService.AddEnrollment(studentId, courseId, studentId, "Student");
                return Ok(new { message = "Enrollment added successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        
        [HttpPost("teacher")]
        public IActionResult AddEnrollmentByTeacher(int studentId, int courseId)
        {
            try
            {
                var teacherIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(teacherIdClaim))
                {
                    return BadRequest(new { message = "User ID is missing from the claim." });
                }

                var teacherId = int.Parse(teacherIdClaim);
                _enrollmentService.AddEnrollment(studentId, courseId, teacherId, "Teacher");
                return Ok(new { message = "Enrollment added by teacher successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
        
        [HttpPost("admin")]
        public IActionResult AddEnrollmentByAdmin(int studentId, int courseId)
        {
            try
            {
                var adminIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(adminIdClaim))
                {
                    return BadRequest(new { message = "User ID is missing from the claim." });
                }

                var adminId = int.Parse(adminIdClaim);
                _enrollmentService.AddEnrollment(studentId, courseId, adminId, "Admin"); // Role 'Admin' string olarak
                return Ok(new { message = "Enrollment added by admin successfully" });
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
                // Kullanıcının rolünü al
                var roleClaim = User.FindFirst(ClaimTypes.Role)?.Value;

                if (roleClaim == "Student") // Eğer kullanıcı öğrenci ise
                {
                    // Kullanıcının ID'sini al
                    var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                    if (string.IsNullOrEmpty(userIdClaim))
                    {
                        return BadRequest(new { message = "User ID is missing from the claim." });
                    }

                    var userId = int.Parse(userIdClaim);
                    var enrollments = _enrollmentService.GetEnrollmentsByStudent(userId); // UserID ile enrollments'ı al
                    return Ok(enrollments);
                }
                else if (roleClaim == "Admin") // Eğer kullanıcı admin ise
                {
                    var enrollments = _enrollmentService.GetAllEnrollments(); // Admin tüm enrollments'ı görebilir
                    return Ok(enrollments);
                }
                else
                {
                    return BadRequest(new { message = "User role is not authorized to view enrollments." });
                }
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
                var studentIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(studentIdClaim))
                {
                    return BadRequest(new { message = "User ID is missing from the claim." });
                }

                var studentId = int.Parse(studentIdClaim);
                _enrollmentService.RemoveEnrollment(id, studentId);
                return Ok(new { message = "Enrollment removed successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}
