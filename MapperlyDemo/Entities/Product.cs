using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MapperlyDemo.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = null;
        public decimal Price { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}