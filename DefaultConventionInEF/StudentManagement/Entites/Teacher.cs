using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StudentManagement.Enum;

namespace StudentManagement.Entites
{
    public class Teacher
    {
        public int TeacherId { get; set; }
        public string FullName { get; set; } = null!;
        public Gender Gender { get; set; }
        public DateTime HireDate { get; set; }
        public decimal Salary { get; set; }
        public virtual Department? Department { get; set; }
        // One Teacher can teach many courses.
        public virtual ICollection<Course> Courses { get; set; } = new List<Course>();
        // One teacher can have zero or many addresses.
        public virtual ICollection<Address> Addresses { get; set; } = new List<Address>();
    }
}