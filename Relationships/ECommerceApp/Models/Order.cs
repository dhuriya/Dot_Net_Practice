using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerceApp.Models
{
    public class Order: BaseAuditableEntity
    {
        public int OrderId {get; set;}
        public DateTime OrderDate {get; set;}
        public int OrderStatusId {get; set;}
        public virtual OrderStatus OrderStatus { get; set; } = null!;

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount {get; set;}
        public int? ShippingAddressId {get; set;}
        public virtual Address? ShippingAddress {get; set;}
        public int? BillingAddressId {get; set;}
        public virtual Address? BillingAddress {get; set;}
        public int CustomerId {get; set;}
        public virtual Customer Customer { get; set; } = null!;
        public virtual ICollection<OrderItem> OrderItems {get; set;} = null!;
    }
}