using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentManagement.Entites
{
    public class Address
    {
        public string Street { get; set; } = null!;
        public string City { get; set; } = null!;
        public string PostalCode { get; set; } = null!;
        public string? State { get; set; }
        public string Country { get; set; } = null!;
        public int? StudentId { get; set; }
        public virtual Student? Student { get; set; }
        public int? TeacherId { get; set; }
        public virtual Teacher? Teacher { get; set; }
    }
}