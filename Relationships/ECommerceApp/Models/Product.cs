using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerceApp.Models
{
    public class Product: BaseAuditableEntity
    {
        public int ProductId {get; set;}
        public string Name {get; set;} = null!;
        [Column(TypeName ="decimal(18,2)")]
        public decimal Price {get; set;}
        public int Stock{get; set;}
        public string SKU {get; set;}=null!;
        public string? Description {get; set;}
        public int CategoryId {get; set;}
        public virtual Category Category {get; set;} = null!;
        //Many to Many
        public virtual ICollection<OrderItem>? OrderItems {get; set;}
    }
}