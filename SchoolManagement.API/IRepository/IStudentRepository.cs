using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SchoolManagement.API.Model;

namespace SchoolManagement.API.IRepository
{
    public interface IStudentRepository
    {
        Task<Student> CreateAsync(Student student);
        Task<IEnumerable<Student>> getAllStudentsAsync();
    }
}