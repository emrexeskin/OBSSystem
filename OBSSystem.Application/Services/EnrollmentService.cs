using OBSSystem.Application.Interfaces;
using OBSSystem.Core.Entities;
using System.Collections.Generic;

namespace OBSSystem.Application.Services
{
    public class EnrollmentService
    {
        private readonly IEnrollmentRepository _enrollmentRepository;

        public EnrollmentService(IEnrollmentRepository enrollmentRepository)
        {
            _enrollmentRepository = enrollmentRepository;
        }

        public void AddEnrollment(int studentId, int courseId)
        {
            var enrollment = new Enrollment
            {
                StudentID = studentId,
                CourseID = courseId
            };
            _enrollmentRepository.AddEnrollment(enrollment);
        }

        public void RemoveEnrollment(int enrollmentId)
        {
            _enrollmentRepository.RemoveEnrollment(enrollmentId);
        }

        public IEnumerable<Enrollment> GetEnrollmentsByStudent(int studentId)
        {
            return _enrollmentRepository.GetEnrollmentsByStudent(studentId);
        }
    }
}
