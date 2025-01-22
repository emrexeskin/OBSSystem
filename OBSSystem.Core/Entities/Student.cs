using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OBSSystem.Core.Entities
{
    public class Student : User
    {
        public int EnrollmentYear { get; set; }
        public int DepartmentID { get; set; } // Bölüm ID'si

        // Navigation Properties
        public Department Department { get; set; } // Öğrencinin bölümü
        public ICollection<Grade> Grades { get; set; }
        public ICollection<Attendance> Attendances { get; set; }
        public ICollection<Enrollment> Enrollments { get; set; }
    }



}
