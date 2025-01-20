using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OBSSystem.Core.Entities
{
    public class Teacher : User
    {
        public string Department { get; set; }

        // Navigation Property
        public ICollection<Course> Courses { get; set; } // Öğretmenin verdiği dersler
    }

}

