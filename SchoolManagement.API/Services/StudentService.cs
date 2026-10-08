using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SchoolManagement.API.Interface;
using SchoolManagement.API.IRepository;
using SchoolManagement.API.Model;

namespace SchoolManagement.API.Services
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepository;

        public StudentService(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }
        public async Task<Student> CreateAysnc(Student student)
        {
            throw new NotImplementedException();
        }
    }
}