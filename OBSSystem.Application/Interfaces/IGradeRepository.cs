using OBSSystem.Core.Entities;
using System.Collections.Generic;

namespace OBSSystem.Application.Interfaces
{
    public interface IGradeRepository
    {
        void AddGrade(Grade grade);
        void UpdateGrade(Grade grade);
        IEnumerable<Grade> GetGradesByStudent(int studentId);
        IEnumerable<Grade> GetGradesByCourse(int courseId);
    }
}
