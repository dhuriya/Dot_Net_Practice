using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LearnCICD.Models;

namespace LearnCICD.Services
{
    public class StudentService : IStudentService
    {
        private static readonly List<Student> Students = new ()
        {
            new Student{id = 1,Name = "Rahul Sharma",Email = "rahul@gmail.com",Course = "C# .NET"},
            new Student{id = 2,Name = "Priya Verma",Email = "priya@gmail.com",Course = "ASP.NET Core"},
            new Student{id = 3,Name = "Amit Kumar",Email = "amit@gmail.com",Course = "SQL Server"},
            new Student{id = 4,Name = "Neha Singh",Email = "neha@gmail.com",Course = "Angular"},
            new Student{id = 5,Name = "Vikas Gupta",Email = "vikas@gmail.com",Course = "Web API"}
        };
        public Student Add(Student student)
        {
            student.id = Students.Max(x => x.id) + 1;
            Students.Add(student);
            return student;
        }

        public List<Student> GetAll()
        {
            return Students;
        }

        public Student? GetById(int id)
        {
            return Students.FirstOrDefault(x => x.id == id);
        }
    }
}