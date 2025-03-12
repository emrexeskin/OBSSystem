using OBSSystem.Core.Entities;
using System.Collections.Generic;

namespace OBSSystem.Application.Interfaces
{
    public interface IEnrollmentRepository
    {
        void AddEnrollment(Enrollment enrollment);
        void RemoveEnrollment(int enrollmentId);
        Enrollment GetEnrollment(int enrollmentId);
        IEnumerable<Enrollment> GetEnrollmentsByStudent(int studentId);
        IEnumerable<Enrollment> GetAllEnrollments();
    }
}
