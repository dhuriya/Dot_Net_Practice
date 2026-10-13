using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MapperlyOrderDemo.DTOs
{
    public class OrderItemResponseDTO
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public decimal LineTotal { get; set; }
    }
}