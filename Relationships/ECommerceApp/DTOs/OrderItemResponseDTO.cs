using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerceApp.DTOs
{
    public class OrderItemResponseDTO
    {
        public string ProductName { get; set; }= null!;
        public string Category{ get; set; } = null!;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}