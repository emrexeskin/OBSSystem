using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OBSSystem.Application.DTOs.Course
{
    public class UpdateCourseDto
    {
        public string CourseName { get; set; }
        public int? TeacherID { get; set; }
        public string Schedule { get; set; }
    }

}
