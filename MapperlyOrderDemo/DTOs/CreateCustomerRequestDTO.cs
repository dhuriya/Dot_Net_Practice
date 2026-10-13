using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Identity.Client.NativeInterop;

namespace MapperlyOrderDemo.DTOs
{
    public class CreateCustomerRequestDTO
    {
        [Required(ErrorMessage ="Full Name is required")]
        [StringLength(100, ErrorMessage ="Full Name cannot exceed 100 characters.")]
        public string FullName { get; set; } = null!;
        [Required(ErrorMessage ="Mobile number is required.")]
        [RegularExpression(@"^[6-9\d\d{9}]$", ErrorMessage ="Mobile must be a valid 10-digit")]
        public string Mobile { get; set; } = null!;
        [EmailAddress(ErrorMessage ="Email format is invalid.")]
        public string? Email { get; set; }
        //Primitive fields (Primitive -> Complex Address)
        [Required(ErrorMessage ="Address Line1 is required.")]
        public string AddressLine1 { get; set; } = null!;
        [Required(ErrorMessage ="city is required.")]
        public string City { get; set; } =  null!;
        [Required(ErrorMessage ="State is requried.")]
        public string State { get; set; } = null!;
        [Required(ErrorMessage ="Pincode is required.")]
        [RegularExpression(@"^\d\d{6}$",ErrorMessage ="Pincode must be 6 digits.")]
        public string Pincode { get; set; } = null!;
    }
}