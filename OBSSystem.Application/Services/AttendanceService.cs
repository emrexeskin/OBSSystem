﻿using OBSSystem.Application.Interfaces;
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

        public void UpdateAttendanceBulk(int courseId, IEnumerable<Attendance> attendances, int teacherId)
        {
            // Öğretmenin bu derse ait yetkisi kontrol ediliyor
            var course = _courseRepository.GetCourseById(courseId);
            if (course == null || course.TeacherID != teacherId)
                throw new UnauthorizedAccessException("You are not authorized to update attendance for this course.");

            foreach (var attendance in attendances)
            {
                _attendanceRepository.UpdateAttendance(attendance);
            }
        }


        // Öğrencinin tüm yoklamalarını getirme
        public IEnumerable<Attendance> GetAttendanceByStudent(int studentId)
        {
            return _attendanceRepository.GetAttendanceByStudent(studentId);
        }
    }
}
