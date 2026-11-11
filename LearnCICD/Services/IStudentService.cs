using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LearnCICD.Models;

namespace LearnCICD.Services
{
    public interface IStudentService
    {
        List<Student> GetAll();
        Student? GetById(int id);
        Student Add(Student student);
    }
}