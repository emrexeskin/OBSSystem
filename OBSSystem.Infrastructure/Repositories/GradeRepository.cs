using OBSSystem.Application.Interfaces;
using OBSSystem.Core.Entities;
using OBSSystem.Infrastructure.Configurations;
using System.Collections.Generic;
using System.Linq;

namespace OBSSystem.Infrastructure.Repositories
{
    public class GradeRepository : IGradeRepository
    {
        private readonly OBSContext _context;

        public GradeRepository(OBSContext context)
        {
            _context = context;
        }

        public void AddGrade(Grade grade)
        {
            _context.Grades.Add(grade);
            _context.SaveChanges();
        }

        public void UpdateGrade(Grade grade)
        {
            _context.Grades.Update(grade);
            _context.SaveChanges();
        }

        public IEnumerable<Grade> GetGradesByStudent(int studentId)
        {
            return _context.Grades.Where(g => g.StudentID == studentId).ToList();
        }

        public IEnumerable<Grade> GetGradesByCourse(int courseId)
        {
            return _context.Grades.Where(g => g.CourseID == courseId).ToList();
        }
    }
}
