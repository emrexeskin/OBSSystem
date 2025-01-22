using OBSSystem.Application.Interfaces;
using OBSSystem.Core.Entities;
using OBSSystem.Infrastructure.Configurations;
using System.Collections.Generic;
using System.Linq;

namespace OBSSystem.Infrastructure.Repositories
{
    public class AttendanceRepository : IAttendanceRepository
    {
        private readonly OBSContext _context;

        public AttendanceRepository(OBSContext context)
        {
            _context = context;
        }

        public void AddAttendance(Attendance attendance)
        {
            _context.Attendances.Add(attendance);
            _context.SaveChanges();
        }

        public void UpdateAttendance(Attendance attendance)
        {
            _context.Attendances.Update(attendance);
            _context.SaveChanges();
        }

        public IEnumerable<Attendance> GetAttendanceByStudent(int studentId)
        {
            return _context.Attendances.Where(a => a.StudentID == studentId).ToList();
        }

        public IEnumerable<Attendance> GetAttendanceByCourse(int courseId)
        {
            return _context.Attendances.Where(a => a.CourseID == courseId).ToList();
        }
    }
}
