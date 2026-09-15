using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using JwtTokenBaseAuthentication.DTO;
using JwtTokenBaseAuthentication.Services.IServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace JwtTokenBaseAuthentication.Services
{
    // here we can oops concept that is encapsulation
    public class UserServices : IUserService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        public UserServices(ApplicationDbContext context,IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<UserResponseDto> Register(UserRegisterDto userRegisterDto)
        {
            // here we can do the manual mappings
            var user = new User
            {
                Name = userRegisterDto.Name,
                Email = userRegisterDto.Email,
                Username = userRegisterDto.Username,
                Role = userRegisterDto.Role,
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
        public async Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u =>u.Username == loginRequestDto.UserName);
            if(user == null)
            {
                throw new Exception("User not found");
            }
            var token = GenerateToken(user);
            return new LoginResponseDto
            {
                Token = token,
                User = new UserResponseDto
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    Username = user.Username
                }
            };
        }
        private string GenerateToken(User user)
        {
            var jwtSetting = _configuration.GetSection("Jwt");
            // here we need to convert the secret key to byte array
            var secretkey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSetting["Key"]));
            // header
            // we can create signing credentials using the secret key and the hashing alogrithm
            var signingCredentials = new SigningCredentials(secretkey, SecurityAlgorithms.HmacSha256);
            // Payload for the payload we can add the claims that we want to include in the token
            var claims = new[]
            {
                new Claim("Id", user.Id.ToString()),
                new Claim(ClaimTypes.Name,user.Name),
                new Claim(ClaimTypes.Role, user.Role),
                new Claim("Username",user.Username),
                new Claim(ClaimTypes.Email, user.Email),
            };
            //Signature
            //create the token
            var token = new JwtSecurityToken(
                issuer: jwtSetting["Issure"],
                audience: jwtSetting["Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: signingCredentials
            );
            // convert the token object to string and return it
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}