﻿namespace OBSSystem.Application.DTOs.Course
{
    public class CreateCourseDto
    {
        public string CourseName { get; set; }
        public int TeacherID { get; set; }
        public string Schedule { get; set; }


        // TeacherName burada olmamalı ya da zorunlu olmamalı
    }
}