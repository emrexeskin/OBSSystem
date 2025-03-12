using OBSSystem.Application.DTOs.Course;
using OBSSystem.Application.Interfaces;
using OBSSystem.Core.Entities;
using System;
using System.Collections.Generic;

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
                Teacher = teacher,  // BURADA ÖĞRETMENİ AYARLIYORUZ
                Schedule = courseDto.Schedule
            };

            _courseRepository.CreateCourse(course);
    
            teacher.Courses.Add(course);
    
            _userRepository.UpdateUser(teacher);
    
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

            Teacher teacher = null;
            if (course.TeacherID.HasValue)
            {
                teacher = _userRepository.GetUserById(course.TeacherID.Value) as Teacher;
            }

            if (teacher == null)
            {
                throw new Exception("Invalid teacher ID");
            }

            // Eski öğretmeni alıyoruz
            Teacher oldTeacher = null;

            // TeacherID nullable olduğu için, önce null kontrolü yapıyoruz
            if (course.TeacherID.HasValue)
            {
                oldTeacher = _userRepository.GetUserById(course.TeacherID.Value) as Teacher;
            }

            // Eğer öğretmen değişmişse
            if (course.TeacherID != courseDto.TeacherID)
            {
                // Eski öğretmeni courses listesinden çıkartıyoruz
                if (oldTeacher != null)
                {
                    oldTeacher.Courses.Remove(course);
                    _userRepository.UpdateUser(oldTeacher); // Eski öğretmeni güncelliyoruz
                }
                // Yeni öğretmeni courses listesine ekliyoruz
                teacher.Courses.Add(course);
               
            }

            // Kursu güncelliyoruz
            course.CourseName = courseDto.CourseName;
            course.Schedule = courseDto.Schedule;

            
            course.TeacherID = courseDto.TeacherID;

            course.Teacher = teacher;
            _courseRepository.UpdateCourse(course);
            _userRepository.UpdateUser(teacher);
            return new CourseDto
            {
                CourseID = course.CourseID,
                CourseName = course.CourseName,
                TeacherID = course.TeacherID,
                TeacherName = teacher.Name,
                Schedule = course.Schedule
            };
        }






        public void DeleteCourse(int courseId)
        {
            var course = _courseRepository.GetCourseById(courseId);
            if (course == null)
            {
                throw new Exception("Course not found");
            }

            // Silinecek dersi öğretmenin derslerinden çıkaralım
            Teacher teacher = null;
            if (course.TeacherID.HasValue)
            {
                teacher = _userRepository.GetUserById(course.TeacherID.Value) as Teacher;
            }

            if (teacher != null)
            {
                teacher.Courses.Remove(course); // Teacher'ın course listesinde bu dersi çıkarıyoruz
                _userRepository.UpdateUser(teacher); // Teacher'ı güncelliyoruz
            }

            // Dersin kendisini siliyoruz
            _courseRepository.DeleteCourse(course);
        }


    }
}
