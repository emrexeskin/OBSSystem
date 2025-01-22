using OBSSystem.Application.Interfaces;
using OBSSystem.Core.Entities;
using System.Collections.Generic;

namespace OBSSystem.Application.Services
{
    public class AttendanceService
    {
        private readonly IAttendanceRepository _attendanceRepository;
        private readonly ICourseRepository _courseRepository;

        public AttendanceService(IAttendanceRepository attendanceRepository, ICourseRepository courseRepository)
        {
            _attendanceRepository = attendanceRepository;
            _courseRepository = courseRepository;
        }

        public void AddAttendance(Attendance attendance, int teacherId)
        {
            // Yetkilendirme: Öğretmen bu dersin sahibi mi?
            var course = _courseRepository.GetCourseById(attendance.CourseID);
            if (course == null || course.TeacherID != teacherId)
            {
                throw new UnauthorizedAccessException("You are not authorized to add attendance for this course.");
            }

            // Yoklamayı ekle
            _attendanceRepository.AddAttendance(attendance);
        }

        // Öğrencinin tüm yoklamalarını getirme
        public IEnumerable<Attendance> GetAttendanceByStudent(int studentId)
        {
            return _attendanceRepository.GetAttendanceByStudent(studentId);
        }
    }
}
