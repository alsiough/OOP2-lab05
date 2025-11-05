using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using StudentApp.Interfaces;
using StudentApp.Models;

namespace StudentApp.DAL
{
    public class FileStudentRepository : IStudentRepository
    {
        private readonly string _filePath;
        private readonly JsonSerializerOptions _options;

        public FileStudentRepository(string filePath)
        {
            _filePath = filePath;
            _options = new JsonSerializerOptions { WriteIndented = true };
        }

        public IEnumerable<Student> LoadAll()
        {
            if (!File.Exists(_filePath)) return new List<Student>();
            var text = File.ReadAllText(_filePath, Encoding.Unicode);
            return string.IsNullOrWhiteSpace(text)
                ? new List<Student>()
                : JsonSerializer.Deserialize<List<Student>>(text, _options) ?? new List<Student>();
        }

        public void SaveAll(IEnumerable<Student> students)
        {
            var json = JsonSerializer.Serialize(students, _options);
            File.WriteAllText(_filePath, json, Encoding.Unicode);
        }
    }
}
