using OBSSystem.Application.Interfaces;
using OBSSystem.Core.Entities;
using OBSSystem.Infrastructure.Configurations;
using System.Collections.Generic;
using System.Linq;

namespace OBSSystem.Infrastructure.Repositories
{
    public class EnrollmentRepository : IEnrollmentRepository
    {
        private readonly OBSContext _context;

        public EnrollmentRepository(OBSContext context)
        {
            _context = context;
        }

        public void AddEnrollment(Enrollment enrollment)
        {
            bool exists = _context.Enrollments
                .Any(e => e.StudentID == enrollment.StudentID && e.CourseID == enrollment.CourseID);

            if (exists)
            {
                throw new InvalidOperationException("Student is already enrolled in this course.");
            }

            _context.Enrollments.Add(enrollment);
            _context.SaveChanges();
        }

        public void RemoveEnrollment(int enrollmentId)
        {
            var enrollment = _context.Enrollments.Find(enrollmentId);
            if (enrollment == null)
            {
                throw new KeyNotFoundException("Enrollment not found.");
            }

            _context.Enrollments.Remove(enrollment);
            _context.SaveChanges();
        }

        public Enrollment GetEnrollment(int enrollmentId)
        {
            return _context.Enrollments.SingleOrDefault(e => e.EnrollmentID == enrollmentId);
        }

        public IEnumerable<Enrollment> GetEnrollmentsByStudent(int studentId)
        {
            return _context.Enrollments
                .Where(e => e.StudentID == studentId)  // StudentID'yi UserID olarak alıyoruz
                .ToList();
        }

        // Yeni metodun implementasyonu
        public IEnumerable<Enrollment> GetAllEnrollments()
        {
            return _context.Enrollments.ToList(); // Tüm enrollments'ı getir
        }
    }
}

