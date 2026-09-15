using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JwtTokenBaseAuthentication.DTO
{
    public class UserRegisterDto
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Username { get; set; }
        public string Role{get;set;}
        //Securely store the password in the database.
        public string?  Password { get; set; }
    }
}