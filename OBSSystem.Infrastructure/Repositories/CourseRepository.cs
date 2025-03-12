using OBSSystem.Application.Interfaces;
using OBSSystem.Core.Entities;
using OBSSystem.Infrastructure.Configurations;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace OBSSystem.Infrastructure.Repositories
{
    public class CourseRepository : ICourseRepository
    {
        private readonly OBSContext _context;

        public CourseRepository(OBSContext context)
        {
            _context = context;
        }

        public void CreateCourse(Course course)
        {
            _context.Courses.Add(course);
            _context.SaveChanges();
        }

        // Kursları çekerken Teacher bilgilerini dahil ediyoruz
        public IEnumerable<Course> GetAllCourses()
        {
            return _context.Courses
                .Include(c => c.Teacher) // Teacher'ı dahil ediyoruz
                .ToList();
            
        }

        public Course GetCourseById(int id)
        {
            return _context.Courses
                .Include(c => c.Teacher) // Teacher bilgilerini dahil ediyoruz
                .SingleOrDefault(c => c.CourseID == id);
        }

        public void UpdateCourse(Course course)
        {
            _context.Courses.Update(course);
            _context.SaveChanges();
        }

        public void DeleteCourse(Course course)
        {
            _context.Courses.Remove(course);
            _context.SaveChanges();
        }
    }
}
