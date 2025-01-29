using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OBSSystem.Application.Services;

namespace OBSSystem.API.Controllers
{
    [Authorize(Roles = "Student")]
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly GradeService _gradeService;
        private readonly AttendanceService _attendanceService;

        public StudentController(GradeService gradeService, AttendanceService attendanceService)
        {
            _gradeService = gradeService;
            _attendanceService = attendanceService;
        }

        [HttpGet("grades")]
        public IActionResult GetGrades()
        {
            try
            {
                var studentIdClaim = User.FindFirst("studentId")?.Value;
                if (string.IsNullOrEmpty(studentIdClaim))
                {
                    return Unauthorized("Student ID not found in the token");
                }

                var studentId = int.Parse(studentIdClaim);
                var grades = _gradeService.GetGradesByStudent(studentId);

                return Ok(grades);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("attendance")]
        public IActionResult GetAttendance()
        {
            try
            {
                var studentIdClaim = User.FindFirst("studentId")?.Value;
                if (string.IsNullOrEmpty(studentIdClaim))
                {
                    return Unauthorized("Student ID not found in the token");
                }

                var studentId = int.Parse(studentIdClaim);
                var attendance = _attendanceService.GetAttendanceByStudent(studentId);

                return Ok(attendance);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("grades/average")]
        public IActionResult GetStudentGradeAverage()
        {
            try
            {
                var studentId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                var average = _gradeService.GetStudentGradeAverage(studentId);
                return Ok(new { average });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

    }
}
