using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OBSSystem.Core.Entities
{
    public class Department
    {
        public int DepartmentID { get; set; } // Primary Key
        public string DepartmentName { get; set; } // Bölüm Adı
        public string Faculty { get; set; } // Fakülte Adı (isteğe bağlı)

        // Navigation Properties
        public ICollection<Student> Students { get; set; } // Bölümdeki öğrenciler
    }

}
