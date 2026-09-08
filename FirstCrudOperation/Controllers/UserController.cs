using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FirstCrudOperation.Models;
using Microsoft.AspNetCore.Mvc;

namespace FirstCrudOperation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        // what is the contsructor 
        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }
        //What are the http method
        // http get
        // http post
        // http put
        // http delete
        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            var users = _context.Users.ToList();
            return Ok(users);
        }
        [HttpGet("GetById/{id}")]
        public IActionResult GetById(int id)
        {
            var user = _context.Users.Find(id);
            return Ok(user);
        }
        // [HttpPost("AddUser")]
        // public IActionResult AddUser(User user)
        // {
            
        // }
    }
}