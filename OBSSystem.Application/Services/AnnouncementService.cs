using OBSSystem.Application.Interfaces;
using OBSSystem.Core.Entities;
using System.Collections.Generic;

namespace OBSSystem.Application.Services
{
    public class AnnouncementService
    {
        private readonly IAnnouncementRepository _announcementRepository;

        public AnnouncementService(IAnnouncementRepository announcementRepository)
        {
            _announcementRepository = announcementRepository;
        }

        public void CreateAnnouncement(Announcement announcement)
        {
            if (announcement.ReceiverType.ToString() == "Course" && !announcement.CourseID.HasValue)
            {
                throw new ArgumentException("CourseID is required for course-specific announcements.");
            }
            _announcementRepository.CreateAnnouncement(announcement);
        }



        public IEnumerable<Announcement> GetAnnouncementsForUser(int userId, string role)
        {
            return _announcementRepository.GetAnnouncementsForUser(userId, role);
        }
    }
}
