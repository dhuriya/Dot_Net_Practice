using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerceApp.Models
{
    public abstract class BaseAuditableEntity
    {
        public bool IsActive {get; set; } = true;
        public DateTime CreateAt {get; set;} = DateTime.UtcNow;
        public DateTime? UpdatedAt {get;set;}
        public string? CreatedBy {get;set;}
        public string? UpdatedBy {get;set;}
    }
}