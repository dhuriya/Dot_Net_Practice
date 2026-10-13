using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Threading.Tasks;

namespace MapperlyOrderDemo.DTOs
{
    public class OrderResponseDTO
    {
        public int Id { get; set; }
        //Complex -> primitive flatteding
        public string CustomerName { get; set; } = null!;
        public decimal TotalAmount { get; set; }
        public string Status { get; set; } = null!;
        public DateTime CreatedOn { get; set; } 
        //ShippingAddress flatening
        public string ShippingCity { get; set; } = null!;
        public string ShippingState { get; set; } = null!;
        public List<OrderItemResponseDTO> Items { get; set; } = new();
    }
}