using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JwtTokenBaseAuthentication
{
    public class User
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Username { get; set; }
        //Securely store the password in the database.
        public string?  Password { get; set; }
        public string Role{get;set;}
    }
    
}
//Dto -- we can use for the security reason because we don't exposing our domain tables