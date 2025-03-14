﻿namespace OBSSystem.Core.Entities
{
    public class Enrollment
    {
        public int EnrollmentID { get; set; } // Benzersiz kayıt kimliği
        public int CourseID { get; set; } // Ders kimliği
        public int StudentID { get; set; }
        // Navigasyon Özellikleri
        public Student Student { get; set; }
        public Course Course { get; set; }
    }
}
