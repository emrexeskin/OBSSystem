using OBSSystem.Application.Interfaces;
using OBSSystem.Core.Entities;
using System.Collections.Generic;

namespace OBSSystem.Application.Services
{
    public class EnrollmentService
    {
        private readonly IEnrollmentRepository _enrollmentRepository;
        private readonly ICourseRepository _courseRepository;
        private readonly IUserRepository _userRepository;
        public EnrollmentService(IEnrollmentRepository enrollmentRepository, ICourseRepository courseRepository, IUserRepository userRepository)
        {
            _enrollmentRepository = enrollmentRepository;
            _courseRepository = courseRepository;
           
        }

        public void AddEnrollment(int studentId, int courseId, int userId, string role)
        {
            if (role == "Teacher")
            {
                var teacherCourse = _courseRepository.GetCourseById(courseId);
                if (teacherCourse == null || teacherCourse.TeacherID != userId)
                {
                    throw new UnauthorizedAccessException("Teachers can only enroll students in their own courses.");
                }
            }
            else if (role == "Student" && studentId != userId)
            {
                throw new UnauthorizedAccessException("Students can only enroll themselves.");
            }

            // Dersin var olup olmadığını kontrol et
            var course = _courseRepository.GetCourseById(courseId);
            if (course == null)
            {
                throw new KeyNotFoundException("Course not found.");
            }

            // Öğrencinin daha önce bu derse kaydolup kaydolmadığını kontrol et
            var existingEnrollment = _enrollmentRepository.GetEnrollmentsByStudent(studentId)
                .FirstOrDefault(e => e.CourseID == courseId);
            if (existingEnrollment != null)
            {
                throw new InvalidOperationException("Student is already enrolled in this course.");
            }

            // Yeni enrollment kaydını oluştur
            var enrollment = new Enrollment
            {
                StudentID  = studentId,
                CourseID = courseId,
                Course = course  // Dersle ilişkilendir
            };

            // Repository'ye kaydet
            _enrollmentRepository.AddEnrollment(enrollment);
        }
        

        public void RemoveEnrollment(int enrollmentId, int studentId)
        {
            var enrollment = _enrollmentRepository.GetEnrollment(enrollmentId);
            if (enrollment == null)
            {
                throw new KeyNotFoundException("Enrollment not found.");
            }

            if (enrollment.StudentID != studentId)
            {
                throw new UnauthorizedAccessException("You are not authorized to remove this enrollment.");
            }

            _enrollmentRepository.RemoveEnrollment(enrollmentId);
        }
        
        public IEnumerable<Enrollment> GetEnrollmentsByStudent(int studentId)
        {
            return _enrollmentRepository.GetEnrollmentsByStudent(studentId);
        }
        
        public IEnumerable<Enrollment> GetAllEnrollments()
        {
            return _enrollmentRepository.GetAllEnrollments();
        }
        
        
    }
}
