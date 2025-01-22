using OBSSystem.Core.Entities;
using System.Collections.Generic;

namespace OBSSystem.Application.Interfaces
{
    public interface IAttendanceRepository
    {
        void AddAttendance(Attendance attendance);
        void UpdateAttendance(Attendance attendance);
        IEnumerable<Attendance> GetAttendanceByStudent(int studentId);
        IEnumerable<Attendance> GetAttendanceByCourse(int courseId);
    }
}
