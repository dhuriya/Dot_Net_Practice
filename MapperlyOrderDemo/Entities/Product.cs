using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace MapperlyOrderDemo.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Desciption { get; set; }
        [Precision(18,2)]
        public decimal Price { get; set; }
        public bool IsActive { get; set; } = true;
    }
}