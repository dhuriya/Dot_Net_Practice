using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerceApp.DTOs
{
    public class OrderRequestDTO
    {
        [Required(ErrorMessage = "CustomerId is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "CustomerId must be a positive integer.")]
        public int CustomerId { get; set; }
        [Required(ErrorMessage = "ShippingAddressId is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "ShippingAddressId must be a positive integer.")]
        public int ShippingAddressId { get; set; }
        [Required(ErrorMessage = "BillingAddressId is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "BillingAddressId must be a positive integer.")]
        public int BillingAddressId { get; set; }
        [Required(ErrorMessage = "Order must contain at least one item.")]
        [MinLength(1, ErrorMessage = "Order must contain at least one item.")]
        public ICollection<OrderItemRequestDTO> OrderItems { get; set; } = new List<OrderItemRequestDTO>();
    }
}