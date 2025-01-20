using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OBSSystem.Core.Entities
{
    public class Announcement
    {
        public int AnnouncementID { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public int CreatedBy { get; set; } // TeacherID or AdminID
        public DateTime DateCreated { get; set; }
    }
}

