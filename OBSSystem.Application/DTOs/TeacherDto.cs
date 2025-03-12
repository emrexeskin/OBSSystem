using OBSSystem.Application.DTOs.Course;
namespace OBSSystem.API.DTOs

{
    public class TeacherDto
    {
        public int UserID { get; set; } 
        public string Name { get; set; }
        public string Department { get; set; }
        public List<CourseDto> Courses { get; set; }
    }
}
