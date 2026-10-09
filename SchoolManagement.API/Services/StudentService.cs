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
        public Task<Student> CreateAsync(Student student)
        {
            return _studentRepository.CreateAsync(student);
        }

        public Task<IEnumerable<Student>> getAllStudentsAsync()
        {
            return _studentRepository.getAllStudentsAsync();
        }
    }
}