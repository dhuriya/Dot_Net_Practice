using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerceApp.Models
{
    public class OrderItem: BaseAuditableEntity
    {
        public int OrderItemId {get; set;}
        public int Quantity {get; set;}
        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; }
        public int OrderId {get; set;}
        public virtual Order Order {get; set;} = null!;
        public int ProductId {get; set;}
        public virtual Product Product {get; set;} = null!;
    }
}