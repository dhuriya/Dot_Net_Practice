using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace MapperlyOrderDemo.Entities
{
    public enum OrderStatus
    {
        Pending = 0,
        Paid = 1,
        Cancelled = 2,
        Shipped = 3,
        Delivered = 4
    }
    public class Order
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public Customer Customer { get; set; } = null!;
        public List<OrderItem> Items { get; set; } = new();
        [Precision(18,2)]
        public decimal TotalAmount { get; set; }
        public bool IsPain { get; set; }
        //Better than string
        public OrderStatus Status {get; set; } = OrderStatus.Pending;
        //Complex Type
        public Address ShippingAddress { get; set; } = new();
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedOn { get; set; } 
        //system fields (ignore in DTOs)
        public string CreatedBy { get; set; } = "SYSTEM";
    }
}