using System;
using System.Collections.Generic;
using System.Linq;
using StudentApp.Interfaces;
using StudentApp.Models;
using StudentApp.Services;

namespace StudentApp.Tests
{
    internal static class StudentServiceTests
    {
        public static void RunAll()
        {
            Console.WriteLine("===== Running StudentService Tests =====\n");

            RunTest("AddStudent_ValidStudent_SavesToRepository", AddStudent_ValidStudent_SavesToRepository);
            RunTest("AddStudent_DuplicateId_Throws", AddStudent_DuplicateId_Throws);
            RunTest("RemoveStudent_WhenExists_Removes", RemoveStudent_WhenExists_Removes);
            RunTest("RemoveStudent_NotFound_Throws", RemoveStudent_NotFound_Throws);
            RunTest("CountForeignExcellentFirstYear_ReturnsCorrectCount", CountForeignExcellentFirstYear_ReturnsCorrectCount);

            Console.WriteLine("\n===== Tests Completed =====");
        }

        private static void RunTest(string name, Action test)
        {
            try
            {
                test();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($" {name} passed");
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($" {name} failed: {ex.Message}");
            }
            finally
            {
                Console.ResetColor();
            }
        }

        private static void AddStudent_ValidStudent_SavesToRepository()
        {
            var repo = new InMemoryRepository();
            var svc = new StudentService(repo);

            var student = new Student { LastName = "Ivanov", Course = 1, AverageGrade = 95, Country = "Kazakhstan" };
            svc.AddStudent(student);

            if (repo.Students.Count != 1)
                throw new Exception("Student was not saved.");
        }

        private static void AddStudent_DuplicateId_Throws()
        {
            var repo = new InMemoryRepository();
            var student = new Student { Id = Guid.NewGuid(), LastName = "A", Course = 1, AverageGrade = 80 };
            repo.Students.Add(student);
            var svc = new StudentService(repo);

            var dup = new Student { Id = student.Id, LastName = "B", Course = 1, AverageGrade = 90 };
            bool thrown = false;

            try { svc.AddStudent(dup); } catch { thrown = true; }

            if (!thrown)
                throw new Exception("Expected exception was not thrown.");
        }

        private static void RemoveStudent_WhenExists_Removes()
        {
            var repo = new InMemoryRepository();
            var s1 = new Student { Id = Guid.NewGuid(), LastName = "Test", Course = 1, AverageGrade = 75 };
            repo.Students.Add(s1);
            var svc = new StudentService(repo);

            svc.RemoveStudent(s1.Id);

            if (repo.Students.Count != 0)
                throw new Exception("Student was not removed.");
        }

        private static void RemoveStudent_NotFound_Throws()
        {
            var repo = new InMemoryRepository();
            var svc = new StudentService(repo);
            bool thrown = false;

            try { svc.RemoveStudent(Guid.NewGuid()); } catch { thrown = true; }

            if (!thrown)
                throw new Exception("Expected exception was not thrown.");
        }

        private static void CountForeignExcellentFirstYear_ReturnsCorrectCount()
        {
            var repo = new InMemoryRepository();
            repo.Students.AddRange(new List<Student>
            {
                new Student { LastName = "A", Course = 1, AverageGrade = 95, Country = "Kazakhstan" },
                new Student { LastName = "B", Course = 1, AverageGrade = 92, Country = "Ukraine" },
                new Student { LastName = "C", Course = 1, AverageGrade = 88, Country = "Poland" },
                new Student { LastName = "D", Course = 2, AverageGrade = 96, Country = "Belarus" }
            });

            var svc = new StudentService(repo);
            int count = svc.CountForeignExcellentFirstYear();

            if (count != 1)
                throw new Exception($"Expected 1, got {count}");
        }
    }

    internal class InMemoryRepository : IStudentRepository
    {
        public List<Student> Students { get; set; } = new List<Student>();

        public IEnumerable<Student> LoadAll() => Students;

        public void SaveAll(IEnumerable<Student> students)
        {
            Students = students.ToList();
        }
    }
}
