using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StudentManagement.Enum;

namespace StudentManagement.Entites
{
    public class Student
    {
        public int StudentId { get; set; }
        public string FirstName { get; set; } = null!;
        public string? LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public bool IsActive { get; set; }
        public byte[]? ProfilePicture { get; set; }
        public Gender Gender { get; set; }
        // One student can have zero or many address
        public virtual ICollection<Address> Addresses { get; set; } = new List<Address>();
        // Many-to-many: one student can enroll in many courses and vice versa.
        public virtual ICollection<Course> Courses { get; set; } = new List<Course>();

    }
}