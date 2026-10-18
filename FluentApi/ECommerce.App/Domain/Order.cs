using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECommerce.App.Enums;

namespace ECommerce.App.Domain
{
    public class Order
    {
        public int Id { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public decimal TotalAmount { get; set; }
        public bool IsDeleted { get; set; }
        public Customer Customer { get; set; } = null!;
        public ICollection<OrderItem> OrderItem { get; set; } = 
        new List<OrderItem>();
    }
}