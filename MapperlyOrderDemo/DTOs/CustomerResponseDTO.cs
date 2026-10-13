using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MapperlyOrderDemo.DTOs
{
    public class CustomerResponseDTO
    {
        public int Id { get; set; }
        public string FullName { get; set; } = null!;
        public string? Email { get; set; }
        //Different name mapping (Mobile -> PhoneNumber)
        public string PhoneNumber { get; set; } = null!;
        // Complex -> Primitive
        public string City { get; set; } =  null!;
        public string State { get; set; } = null!;
        //Fixed/Dynamic mapping examples
        public string SourceSystem { get; set; } = null!;
        public DateTime MappedAtUtc { get; set; }
    }
}