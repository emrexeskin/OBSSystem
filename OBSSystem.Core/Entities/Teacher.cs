using System.Text.Json.Serialization;

namespace OBSSystem.Core.Entities
{
    public class Teacher : User
    {
        public string? Department { get; set; }

        // Navigation Property
        
        public ICollection<Course> Courses { get; set; } // Öğretmenin verdiği dersler
    }

}

