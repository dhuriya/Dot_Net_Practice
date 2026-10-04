using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerceApp.DTOs
{
    public class CustomerResponseDTO
    {
        public int CustomerId { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? Phone { get; set; }
        public string? DisplayName { get; set; }
        public string? Gender { get; set; }
        public string? DateOfBirth { get; set; }
    }
}