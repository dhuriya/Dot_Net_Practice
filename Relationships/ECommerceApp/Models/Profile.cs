using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerceApp.Models
{
    public class Profile: BaseAuditableEntity
    {
        [Key]
        public int CustomerId {get; set;}
        public virtual Customer Customer { get; set;} = null!;
        public string DisplayName {get;set;} = null!;
        public string Gender {get; set;} = null!;
        public DateTime DateOfBirth {get; set;}
    }
}