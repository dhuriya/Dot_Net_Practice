using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerceApp.Models
{
    public class Category: BaseAuditableEntity
    {
        public int CategoryId {get; set;}
        public string Name {get; set;} = null!;
        public string? Description {get; set;}
        // One Category -> many Products
        public virtual ICollection<Product>? Products{get; set;}
    }
}