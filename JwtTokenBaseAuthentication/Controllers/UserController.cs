using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JwtTokenBaseAuthentication.DTO;
using JwtTokenBaseAuthentication.Services.IServices;
using Microsoft.AspNetCore.Mvc;

namespace JwtTokenBaseAuthentication.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        // here we able achive the abstraction
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegisterDto userRegisterDto)
        {
            var result = await _userService.Register(userRegisterDto);
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginRequestDto loginRequestDto)
        {
            try
            {
                var result = await _userService.Login(loginRequestDto);
                return Ok(new
                {
                    message = "Login successful",
                    Token = result.Token,
                    User = result.User
                });
            }catch(Exception ex)
            {
                return BadRequest(new
                {
                    message = "Login failed",
                    error = ex.Message
                });
            }
            finally
            {
                Console.WriteLine("This is the finally block, it will be executed regardles of whether an exception is");
            }
        }
    }
}