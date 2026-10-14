using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagement.Entites
{
    public class Course
    {
        public int CourseId { get; set; }
        public string CourseName { get; set; } = null!;
        public int TeacherId { get; set; }
        public virtual Teacher? Teacher { get; set; }
        public int DepartmentId { get; set; }
        public virtual Department? Department { get; set; }
        // Many-to-many one: Course can have many students enrolled, and one student can join many courses.
        public virtual ICollection<Student> Students { get; set; } = new List<Student>();
    }
}