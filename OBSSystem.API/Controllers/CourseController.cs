using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OBSSystem.API.DTOs;
using OBSSystem.Application.DTOs;
using OBSSystem.Core.Entities;
using OBSSystem.Infrastructure.Configurations;


namespace OBSSystem.API.Controllers


{


    public class CourseRequest
    {
        public string CourseName { get; set; }
        public int TeacherID { get; set; }
        public string Schedule { get; set; }
    }


    [Authorize(Roles = "Admin")] // Only Admin can manage courses
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly OBSContext _context;

        public CourseController(OBSContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult CreateCourse([FromBody] CreateCourseDto courseDto)
        {
            if (courseDto == null)
            {
                return BadRequest(new { message = "Invalid course data" });
            }

            // Verify the teacher exists and has the Teacher role
            var teacher = _context.Users.OfType<Teacher>().SingleOrDefault(t => t.UserID == courseDto.TeacherID);
            if (teacher == null)
            {
                return BadRequest(new { message = "Invalid teacher ID" });
            }

            // Map DTO to Entity
            var course = new Course
            {
                CourseName = courseDto.CourseName,
                TeacherID = courseDto.TeacherID,
                Schedule = courseDto.Schedule
            };

            _context.Courses.Add(course);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetCourseById), new { id = course.CourseID }, new
            {
                course.CourseID,
                course.CourseName,
                course.TeacherID,
                TeacherName = teacher.Name, // Bu alan sadece döndürülecek
                course.Schedule
            });
        }

        [HttpGet]
        public IActionResult GetAllCourses()
        {
            var courses = _context.Courses.Include(c => c.Teacher).ToList();

            var courseDtos = courses.Select(c => new CourseDto
            {
                CourseID = c.CourseID,
                CourseName = c.CourseName,
                TeacherID = c.TeacherID,
                TeacherName = c.Teacher?.Name,
                Schedule = c.Schedule
            }).ToList();

            return Ok(courseDtos);
        }

        [HttpGet("{id}")]
        public IActionResult GetCourseById(int id)
        {
            var course = _context.Courses.Include(c => c.Teacher).SingleOrDefault(c => c.CourseID == id);
            if (course == null)
            {
                return NotFound(new { message = "Course not found" });
            }

            var courseDto = new CourseDto
            {
                CourseID = course.CourseID,
                CourseName = course.CourseName,
                TeacherID = course.TeacherID,
                TeacherName = course.Teacher?.Name,
                Schedule = course.Schedule
            };

            return Ok(courseDto);
        }


        [HttpPut("{id}")]
        public IActionResult UpdateCourse(int id, [FromBody] UpdateCourseDto courseDto)
        {
            if (courseDto == null)
            {
                return BadRequest(new { message = "Invalid course data" });
            }

            // Mevcut dersin olup olmadığını kontrol et
            var existingCourse = _context.Courses.SingleOrDefault(c => c.CourseID == id);
            if (existingCourse == null)
            {
                return NotFound(new { message = "Course not found" });
            }

            // Öğretmenin geçerli bir ID'ye sahip olduğunu kontrol et
            var teacher = _context.Users.OfType<Teacher>().SingleOrDefault(t => t.UserID == courseDto.TeacherID);
            if (teacher == null)
            {
                return BadRequest(new { message = "Invalid teacher ID" });
            }

            // Güncellenen verileri mevcut derse uygula
            existingCourse.CourseName = courseDto.CourseName;
            existingCourse.TeacherID = courseDto.TeacherID;
            existingCourse.Schedule = courseDto.Schedule;

            _context.SaveChanges();

            return Ok(new
            {
                existingCourse.CourseID,
                existingCourse.CourseName,
                existingCourse.TeacherID,
                TeacherName = teacher.Name, // Geri döndürmek için öğretmenin adını da ekleyelim
                existingCourse.Schedule
            });
        }




        [HttpDelete("{id}")]
        public IActionResult DeleteCourse(int id)
        {
            var course = _context.Courses.Find(id);
            if (course == null)
            {
                return NotFound(new { message = "Course not found" });
            }

            _context.Courses.Remove(course);
            _context.SaveChanges();

            return NoContent();
        }
    }

}
