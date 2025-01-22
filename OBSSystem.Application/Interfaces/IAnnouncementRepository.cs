using OBSSystem.Core.Entities;
using System.Collections.Generic;

namespace OBSSystem.Application.Interfaces
{
    public interface IAnnouncementRepository
    {
        void CreateAnnouncement(Announcement announcement);
        IEnumerable<Announcement> GetAnnouncementsForUser(int userId, string role);
    }
}
