using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LearnCICD.Models
{
    public class Student
    {
        public int id {get; set;}
        public string Name {get; set;} = string.Empty;
        public string Email {get; set;} = string.Empty;
        public string Course {get; set;} = string.Empty;
    }
}