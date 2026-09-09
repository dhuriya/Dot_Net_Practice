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
        //Get : api/User/GetAll
        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            //Fetch All Student from Database
            var users = _context.Users.ToList();
            return Ok(users);
        }
        //Get : api/User/1/GetById
        [HttpGet("{id}/GetById")]
        public IActionResult GetById(int id)
        {
            //we  can find user with the help of id
            var user = _context.Users.Find(id);
            if(user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }
        //Post : api/User/Add
        [HttpPost("Add")]
        public IActionResult AddUser(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
            return Ok(user);
        }

        // what is the difference between put and pathc?
        //Put : api/User/2/Update
        [HttpPut("{id}/Update")]
        public IActionResult Update(int id,User user)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            var existUser = _context.Users.Find(id);
            if(existUser == null)
            {
                return NotFound();
            }
            existUser.Name = user.Name;
            existUser.Email = user.Email;
            existUser.Password = user.Password;
            _context.SaveChanges();
            return Ok(existUser);
        }
        //Delete : api/User/2/Delete
        [HttpDelete("{id}/Delete")]
        public IActionResult Delete(int id)
        {
            var existUser = _context.Users.Find(id);
            if(existUser == null)
            {
                return NotFound();
            }
            _context.Users.Remove(existUser);
            _context.SaveChanges();
            return Ok("User will Deleted Successfully.");
        }

    }
}