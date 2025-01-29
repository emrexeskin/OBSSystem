using OBSSystem.Application.Interfaces;
using OBSSystem.Core.Entities;
using System.Collections.Generic;

namespace OBSSystem.Application.Services
{
    public class GradeService
    {
        private readonly IGradeRepository _gradeRepository;
        private readonly ICourseRepository _courseRepository;

        public GradeService(IGradeRepository gradeRepository, ICourseRepository courseRepository)
        {
            _gradeRepository = gradeRepository;
            _courseRepository = courseRepository;
        }

        public void AddGrade(Grade grade, int teacherId)
        {
            // Yetkilendirme: Öğretmen bu dersin sahibi mi?
            var course = _courseRepository.GetCourseById(grade.CourseID);
            if (course == null || course.TeacherID != teacherId)
            {
                throw new UnauthorizedAccessException("You are not authorized to add grades for this course.");
            }

            // Notu ekle
            _gradeRepository.AddGrade(grade);
        }

        public double GetStudentGradeAverage(int studentId)
        {
            var grades = _gradeRepository.GetGradesByStudent(studentId);
            if (!grades.Any())
                throw new Exception("No grades found for this student.");

            return grades.Average(g => g.Score);
        }

        public object GetCourseStatistics(int courseId)
        {
            var grades = _gradeRepository.GetGradesByCourse(courseId);

            return new
            {
                Average = grades.Any() ? grades.Average(g => g.Score) : 0,
                Highest = grades.Any() ? grades.Max(g => g.Score) : 0,
                Lowest = grades.Any() ? grades.Min(g => g.Score) : 0,
                Count = grades.Count()
            };
        }



        // Öğrencinin tüm notlarını getirme
        public IEnumerable<Grade> GetGradesByStudent(int studentId)
        {
            return _gradeRepository.GetGradesByStudent(studentId);
        }
    }
}
