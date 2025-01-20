namespace OBSSystem.API.DTOs
{
    public class CourseDto
    {
        public int CourseID { get; set; }
        public string CourseName { get; set; }
        public int TeacherID { get; set; }
        public string Schedule { get; set; }
        public string? TeacherName { get; set; } // Optional (Nullable)
    }
}
