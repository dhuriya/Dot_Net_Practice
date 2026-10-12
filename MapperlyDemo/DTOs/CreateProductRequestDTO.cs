using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MapperlyDemo.DTOs
{
    public class CreateProductRequestDTO
    {
        public string Name { get; set; } = null;
        public decimal Price { get; set; }
    }
}