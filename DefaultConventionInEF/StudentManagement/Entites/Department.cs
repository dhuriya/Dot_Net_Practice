using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagement.Entites
{
    public class Department
    {
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; } = null!;
        // One department can have zero or many teachers
        public virtual ICollection<Teacher> Teachers { get; set; } = new List<Teacher>();
        // one department can offer zero or many courses.
        public virtual ICollection<Course> Courses { get; set; } = new List<Course>();
    }
}