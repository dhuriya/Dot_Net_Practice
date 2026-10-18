using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.App.DTOs
{
    public class OrderRequestDTO
    {
        [Required(ErrorMessage = "CustomerId is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "CustomerId must be greater than zero.")]
        public int CustomerId { get; set; }
        [Required(ErrorMessage = "Items collection cannot be empty.")]
        [MinLength(1, ErrorMessage = "At least one order item is required.")]
        public List<OrderItemRequestDTO> Items { get; set; } = new();
    }
}