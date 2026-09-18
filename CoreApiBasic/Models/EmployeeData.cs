using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreApiBasic.Models
{
    public static class EmployeeData
    {
        public static List<Employee> Employees { get; } = new List<Employee>
        {
            new Employee { Id = 1, Name = "John Doe", Gender = "Male", Department = "IT", City = "New York" },
            new Employee { Id = 2, Name = "Jane Smith", Gender = "Female", Department = "HR", City = "Los Angeles" },
            new Employee { Id = 3, Name = "Bob Johnson", Gender = "Male", Department = "Finance", City = "Chicago" }
        };
    }
}