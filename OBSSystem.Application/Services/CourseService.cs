using OBSSystem.Application.DTOs.Course;
using OBSSystem.Application.Interfaces;
using OBSSystem.Core.Entities;

namespace OBSSystem.Application.Services
{
    public class CourseService
    {
        private readonly ICourseRepository _courseRepository;
        private readonly IUserRepository _userRepository;

        public CourseService(ICourseRepository courseRepository, IUserRepository userRepository)
        {
            _courseRepository = courseRepository;
            _userRepository = userRepository;
        }

        public CourseDto CreateCourse(CreateCourseDto courseDto)
        {
            var teacher = _userRepository.GetUserById(courseDto.TeacherID) as Teacher;
            if (teacher == null)
            {
                throw new Exception("Invalid teacher ID");
            }

            var course = new Course
            {
                CourseName = courseDto.CourseName,
                TeacherID = courseDto.TeacherID,
                Schedule = courseDto.Schedule
            };

            _courseRepository.CreateCourse(course);

            return new CourseDto
            {
                CourseID = course.CourseID,
                CourseName = course.CourseName,
                TeacherID = course.TeacherID,
                TeacherName = teacher.Name,
                Schedule = course.Schedule
            };
        }

        public IEnumerable<CourseDto> GetAllCourses()
        {
            var courses = _courseRepository.GetAllCourses();
            return courses.Select(c => new CourseDto
            {
                CourseID = c.CourseID,
                CourseName = c.CourseName,
                TeacherID = c.TeacherID,
                TeacherName = c.Teacher?.Name,
                Schedule = c.Schedule
            });
        }

        public CourseDto GetCourseById(int id)
        {
            var course = _courseRepository.GetCourseById(id);
            if (course == null) return null;

            return new CourseDto
            {
                CourseID = course.CourseID,
                CourseName = course.CourseName,
                TeacherID = course.TeacherID,
                TeacherName = course.Teacher?.Name,
                Schedule = course.Schedule
            };
        }

        public CourseDto UpdateCourse(int id, UpdateCourseDto courseDto)
        {
            var course = _courseRepository.GetCourseById(id);
            if (course == null)
            {
                throw new Exception("Course not found");
            }

            var teacher = _userRepository.GetUserById(courseDto.TeacherID) as Teacher;
            if (teacher == null)
            {
                throw new Exception("Invalid teacher ID");
            }

            course.CourseName = courseDto.CourseName;
            course.TeacherID = courseDto.TeacherID;
            course.Schedule = courseDto.Schedule;

            _courseRepository.UpdateCourse(course);

            return new CourseDto
            {
                CourseID = course.CourseID,
                CourseName = course.CourseName,
                TeacherID = course.TeacherID,
                TeacherName = teacher.Name,
                Schedule = course.Schedule
            };
        }

        public void DeleteCourse(int id)
        {
            var course = _courseRepository.GetCourseById(id);
            if (course == null)
            {
                throw new Exception("Course not found");
            }

            _courseRepository.DeleteCourse(course);
        }
    }
}
