using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OBSSystem.Core.Entities
{
    public enum ReceiverType
    {
        All,     // Tüm kullanıcılar
        Course,  // Belirli bir dersin öğrencileri
        Student  // Belirli bir öğrenci
    }
    public class Announcement
    {
        public int AnnouncementID { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int SenderID { get; set; }
        public string ReceiverType { get; set; } // "All", "Course", "Student"
        public int? CourseID { get; set; }
        public DateTime CreatedAt { get; set; }
    }

}

