using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JwtTokenBaseAuthentication.DTO
{
    public class LoginResponseDto
    {
        public UserResponseDto User {get; set;}
        public string Token {get; set;}
    }
}