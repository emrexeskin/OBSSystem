using OBSSystem.Application.Interfaces;
using OBSSystem.Core.Entities;
using OBSSystem.Infrastructure.Configurations;
using System.Collections.Generic;
using System.Linq;

namespace OBSSystem.Infrastructure.Repositories
{
    public class AnnouncementRepository : IAnnouncementRepository
    {
        private readonly OBSContext _context;

        public AnnouncementRepository(OBSContext context)
        {
            _context = context;
        }

        public void CreateAnnouncement(Announcement announcement)
        {
            _context.Announcements.Add(announcement);
            _context.SaveChanges();
        }

        public IEnumerable<Announcement> GetAnnouncementsForUser(int userId, string role)
        {
            if (role == "Student")
            {
                return _context.Announcements.Where(a =>
                    a.ReceiverType == "All" ||
                    (a.ReceiverType == "Course" && a.CourseID.HasValue &&
                     _context.Enrollments.Any(e => e.CourseID == a.CourseID && e.StudentID == userId))
                ).ToList();
            }
            return _context.Announcements.ToList();
        }
    }
}
