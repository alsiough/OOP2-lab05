using System.Collections.Generic;
using StudentApp.Models;

namespace StudentApp.Interfaces
{
    public interface IStudentRepository
    {
        IEnumerable<Student> LoadAll();
        void SaveAll(IEnumerable<Student> students);
    }
}
