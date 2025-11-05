using System;
using System.Collections.Generic;
using System.Linq;
using StudentApp.Interfaces;
using StudentApp.Models;

namespace StudentApp.Services
{
    public class StudentService
    {
        private readonly IStudentRepository _repository;

        public StudentService(IStudentRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<Student> GetAllStudents() => _repository.LoadAll();

        public void AddStudent(Student s)
        {
            ValidateStudent(s);
            var students = _repository.LoadAll().ToList();
            if (students.Any(x => x.Id == s.Id))
                throw new InvalidOperationException("Student with same Id already exists.");
            students.Add(s);
            _repository.SaveAll(students);
        }

        public void RemoveStudent(Guid id)
        {
            var students = _repository.LoadAll().ToList();
            var removed = students.RemoveAll(s => s.Id == id);
            if (removed == 0) throw new KeyNotFoundException("Student not found.");
            _repository.SaveAll(students);
        }

        public int CountForeignExcellentFirstYear()
        {
            return _repository.LoadAll()
                .Count(s => s.Course == 1 && s.IsExcellent() && !IsUkrainian(s));
        }

        private static bool IsUkrainian(Student s)
        {
            return s.Country.Equals("Україна", StringComparison.OrdinalIgnoreCase)
                || s.Country.Equals("Ukraine", StringComparison.OrdinalIgnoreCase);
        }

        private static void ValidateStudent(Student s)
        {
            if (string.IsNullOrWhiteSpace(s.LastName))
                throw new ArgumentException("LastName is required.");
            if (s.Course < 1 || s.Course > 6)
                throw new ArgumentOutOfRangeException(nameof(s.Course));
            if (s.AverageGrade < 0 || s.AverageGrade > 100)
                throw new ArgumentOutOfRangeException(nameof(s.AverageGrade));
        }
    }
}
