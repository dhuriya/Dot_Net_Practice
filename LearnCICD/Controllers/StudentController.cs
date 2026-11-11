using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LearnCICD.Models;
using LearnCICD.Services;
using Microsoft.AspNetCore.Mvc;

namespace LearnCICD.Controllers
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
        [HttpGet]
        public IActionResult GetAll()
        {
            var students = _studentService.GetAll();
            return Ok(students);
        }
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var student = _studentService.GetById(id);
            if(student == null)
            {
                return NotFound($"Student with Id {id} not found");
            }
            return Ok(student);
        }
        [HttpPost]
        public IActionResult Create(Student student)
        {
            var createdStudent = _studentService.Add(student);
            return CreatedAtAction(nameof(GetById), new {id = createdStudent.id}, createdStudent);
        }
    }
}