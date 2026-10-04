using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerceApp.Models
{
    public class OrderStatus: BaseAuditableEntity
    {
        public int OrderStatusId {get; set;}
        public string Name {get; set;}= null!;
        public string? Description{get; set;}
        // One Status can be assigned to many orders
        public virtual ICollection<Order>? Orders{get; set;} 
    }
}