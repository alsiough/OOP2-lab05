using System;
using System.Collections.Generic;

namespace StudentApp.Models
{
    public class Student
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string LastName { get; set; } = string.Empty;
        public int Course { get; set; }
        public string StudentCard { get; set; } = string.Empty;
        public double AverageGrade { get; set; }
        public string Country { get; set; } = string.Empty;
        public string PassportNumber { get; set; } = string.Empty;
        public List<string> Skills { get; set; } = new();

        public bool IsExcellent() => AverageGrade >= 90.0;
    }
}
