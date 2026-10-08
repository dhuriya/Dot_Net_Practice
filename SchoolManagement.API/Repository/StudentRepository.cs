using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SchoolManagement.API.IRepository;
using SchoolManagement.API.Model;

namespace SchoolManagement.API.Repository
{
    public class StudentRepository : IStudentRepository
    {
        private readonly string _connectionString;

        public StudentRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("School")!;
        }
        public async Task<Student> CreateAsync(Student student)
        {
            throw new NotImplementedException();
        }
    }
}