using Microsoft.AspNetCore.Mvc;
using SchoolManagement.API.Interface;
using SchoolManagement.API.Model;

namespace SchoolManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;

        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        [HttpPost("create")]
        public async Task<ActionResult<Student>> CreateAsync([FromBody] Student student)
        {
            try
            {
                var createdStudent = await _studentService.CreateAsync(student);
                return StatusCode(StatusCodes.Status201Created, createdStudent);
            }
            catch (ArgumentException exception)
            {
                return BadRequest(new { error = exception.Message });
            }
        }
        [HttpGet("GetAll")]
        public async Task<ActionResult<IEnumerable<Student>>> GetAllStudents()
        {
            try
            {
                var students = await _studentService.getAllStudentsAsync();
                return StatusCode(StatusCodes.Status200OK,students);
            }catch(Exception ex)
            {
                return BadRequest(new {error = ex.Message});
            }
        }
    }
}