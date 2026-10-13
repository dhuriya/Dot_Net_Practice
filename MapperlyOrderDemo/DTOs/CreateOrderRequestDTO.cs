using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Identity.Client.NativeInterop;

namespace MapperlyOrderDemo.DTOs
{
    public class CreateOrderRequestDTO
    {
        [Range(1, int.MaxValue, ErrorMessage ="CustomerId must be a valid positive number.")]
        public int CustomerId { get; set; }
        [MinLength(1, ErrorMessage ="At least one order item is required.")]
        public List<CreateOrderItemRequestDTO> Items { get; set; } = new();
        //Primitive -> Complex ShippingAddress
        [Required(ErrorMessage ="Shipping Address Lineq is required.")]
        public string ShippingLine1 { get; set; } = null!;
        [Required(ErrorMessage ="Shipping City is required.")]
        public string ShippingCity { get; set; } = null!;
        [Required(ErrorMessage ="Shipping state is required.")]
        public string ShippingState { get; set; }  = null!;
        [Required(ErrorMessage ="Shipping Pincode is required.")]
        [RegularExpression(@"^\d\d{6}$", ErrorMessage ="Shipping Pincode must be 6 digits.")]
        public string ShippingPicode { get; set; } = null!;
    }
}