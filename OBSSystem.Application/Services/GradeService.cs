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

        // Öğrencinin tüm notlarını getirme
        public IEnumerable<Grade> GetGradesByStudent(int studentId)
        {
            return _gradeRepository.GetGradesByStudent(studentId);
        }
    }
}
