using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OBSSystem.Application.Services;
using OBSSystem.Core.Entities;

namespace OBSSystem.API.Controllers
{
    [Authorize(Roles = "Teacher")]
    [Route("api/[controller]")]
    [ApiController]
    public class TeacherController : ControllerBase
    {
        private readonly GradeService _gradeService;
        private readonly AttendanceService _attendanceService;

        public TeacherController(GradeService gradeService, AttendanceService attendanceService)
        {
            _gradeService = gradeService;
            _attendanceService = attendanceService;
        }

        [HttpPost("add-grade")]
        public IActionResult AddGrade([FromBody] Grade grade)
        {
            try
            {
                var teacherIdClaim = User.FindFirst("teacherId")?.Value;
                if (string.IsNullOrEmpty(teacherIdClaim))
                {
                    return Unauthorized("Teacher ID not found in the token");
                }

                var teacherId = int.Parse(teacherIdClaim);
                _gradeService.AddGrade(grade, teacherId);

                return Ok(new { message = "Grade added successfully" });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("add-attendance")]
        public IActionResult AddAttendance([FromBody] Attendance attendance)
        {
            try
            {
                var teacherIdClaim = User.FindFirst("teacherId")?.Value;
                if (string.IsNullOrEmpty(teacherIdClaim))
                {
                    return Unauthorized("Teacher ID not found in the token");
                }

                var teacherId = int.Parse(teacherIdClaim);
                _attendanceService.AddAttendance(attendance, teacherId);

                return Ok(new { message = "Attendance added successfully" });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("courses/{courseId}/statistics")]
        public IActionResult GetCourseStatistics(int courseId)
        {
            try
            {
                var teacherId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                var statistics = _gradeService.GetCourseStatistics(courseId);
                return Ok(statistics);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("courses/{courseId}/attendance")]
        public IActionResult UpdateAttendanceBulk(int courseId, [FromBody] IEnumerable<Attendance> attendances)
        {
            try
            {
                var teacherId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                _attendanceService.UpdateAttendanceBulk(courseId, attendances, teacherId);
                return Ok(new { message = "Attendance updated successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }




    }
}
