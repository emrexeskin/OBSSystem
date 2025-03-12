using OBSSystem.Core.Entities;
using System.Collections.Generic;

namespace OBSSystem.Application.Interfaces
{
    public interface ICourseRepository
    {
        void CreateCourse(Course course);
        IEnumerable<Course> GetAllCourses();
        Course GetCourseById(int id);
        void UpdateCourse(Course course);
        void DeleteCourse(Course course);
        
        
    }
}
