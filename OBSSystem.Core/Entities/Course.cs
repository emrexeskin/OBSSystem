using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OBSSystem.Core.Entities
{
    public class Course
    {
        public int CourseID { get; set; }
        public string CourseName { get; set; }
        public int? TeacherID { get; set; }
        public string Schedule { get; set; } // Example: "Mon 09:00-11:00"
        public ICollection<Enrollment> Enrollments { get; set; }


        // Navigation Property
        public Teacher Teacher { get; set; }
    }

}
