using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MapperlyOrderDemo.Entities
{
    public class Customer
    {
        public int Id { get; set; }
        public string FullName { get; set; } = null!;
        public string Mobile { get; set; } = null!;
        public string? Email { get; set; }
        public Address? Address { get; set; }
        public DateTime RegisteredOn { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; } = true;
        // Real-world audit/system fields
        public string CreatedBy { get; set; } = "SYSTEM";
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedOn { get; set; }

        public List<Order> Orders { get; set; } = new();
    }
}