using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MapperlyOrderDemo.Entities
{
    public class Address
    {
        public string Line1 { get; set; } = null!;
        public string City { get; set; } = null!;
        public string State { get; set; } = null!;
        public string Pincode { get; set; } = null!;
    }
}