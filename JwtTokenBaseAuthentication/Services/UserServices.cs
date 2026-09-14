using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JwtTokenBaseAuthentication.DTO;
using JwtTokenBaseAuthentication.Services.IServices;

namespace JwtTokenBaseAuthentication.Services
{
    // here we can oops concept that is encapsulation
    public class UserServices : IUserService
    {
        private readonly ApplicationDbContext _context;
        public UserServices(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<UserResponseDto> Register(UserRegisterDto userRegisterDto)
        {
            // here we can do the manual mappings
            var user = new User
            {
                Name = userRegisterDto.Name,
                Email = userRegisterDto.Email,
                Username = userRegisterDto.Username,
                Password = userRegisterDto.Password
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            // Domain table User ---- Dto userresponse dto
            return new UserResponseDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Name,
                Username = user.Username
            };
        }
    }
}