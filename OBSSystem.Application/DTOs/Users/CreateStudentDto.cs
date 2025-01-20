using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OBSSystem.Application.DTOs.Create
{
    public class CreateStudentDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; } = "Student"; // Varsayılan değer
        public int DepartmentID { get; set; } // Zorunlu alan
    }
}
