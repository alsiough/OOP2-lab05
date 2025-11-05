using StudentApp.DAL;
using StudentApp.Models;
using StudentApp.Services;
using StudentApp.Tests;
using System;

namespace StudentApp
{
    internal class Program
    {
        static void Main()
        {

           // StudentServiceTests.RunAll();  USE FOR TEST VIEW!!!!!!!!!!!!
           // Console.ReadKey();

            Console.OutputEncoding = System.Text.Encoding.Unicode;
            var repo = new FileStudentRepository("students_utf16.json");
            var service = new StudentService(repo);

            while (true)
            {
                Console.Clear();
                Console.WriteLine("===== Student Management System =====\n");
                Console.WriteLine("1. View all students");
                Console.WriteLine("2. Add student");
                Console.WriteLine("3. Remove student");
                Console.WriteLine("4. Count foreign excellent first-year students");
                Console.WriteLine("0. Exit\n");
                Console.Write("Select: ");

                switch (Console.ReadLine())
                {
                    case "1":
                        ShowStudents(service);
                        break;
                    case "2":
                        AddStudent(service);
                        break;
                    case "3":
                        RemoveStudent(service);
                        break;
                    case "4":
                        Console.WriteLine($"Foreign excellent 1st-year students: {service.CountForeignExcellentFirstYear()}\n");
                        Console.ReadKey();
                        break;
                    case "0":
                        return;
                }
            }
        }

        static void ShowStudents(StudentService service)
        {
            Console.Clear();
            var students = service.GetAllStudents();
            foreach (var s in students)
                Console.WriteLine($"{s.Id} | {s.LastName} | Course: {s.Course} | Grade: {s.AverageGrade} | {s.Country}");
            Console.WriteLine("\nPress any key...");
            Console.ReadKey();
        }

        static void AddStudent(StudentService service)
        {
            Console.Clear();
            Console.WriteLine("Adding new student:\n");
            var s = new Student();

            Console.Write("Last name: "); s.LastName = Console.ReadLine() ?? string.Empty;
            Console.Write("Course: "); s.Course = int.Parse(Console.ReadLine() ?? "1");
            Console.Write("Average grade: "); s.AverageGrade = double.Parse(Console.ReadLine() ?? "0");
            Console.Write("Country: "); s.Country = Console.ReadLine() ?? string.Empty;

            try
            {
                service.AddStudent(s);
                Console.WriteLine("\nSaved successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError: {ex.Message}");
            }
            Console.ReadKey();
        }

        static void RemoveStudent(StudentService service)
        {
            Console.Clear();
            Console.Write("Enter student ID to remove: ");
            var id = Console.ReadLine();
            if (Guid.TryParse(id, out var guid))
            {
                try
                {
                    service.RemoveStudent(guid);
                    Console.WriteLine("Removed successfully.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("Invalid GUID format.");
            }
            Console.ReadKey();
        }
    }
}
