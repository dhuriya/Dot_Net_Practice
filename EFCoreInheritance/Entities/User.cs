using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EFCoreInheritance.Entities
{
    public class User
    {
        public int Id { get; set; }                              
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Username { get; set; } = null!;
        public string PasswordHash { get; set; } = null!;
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string? CreatedBy { get; set; } = "System";
    }
    public class Customer : User
    {
        public string? PhoneNumber { get; set; }
        public string? ShippingAddress { get; set; }
        public string? BillingAddress { get; set; }
        public string? ProfilePictureUrl { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public int LoyaltyPoints { get; set; } = 0;
    }
    public class Seller : User
    {
        public string BusinessName { get; set; } = null!;
        public string GSTNumber { get; set; } = null!;
        public string PANNumber { get; set; } = null!;
        public string WarehouseAddress { get; set; } = null!;
        public string? BusinessLicenseNo { get; set; }
        public string? BankAccountNumber { get; set; }
        public bool IsVerified { get; set; } = false;
        public string? SupportPhone { get; set; }
    }
    public class AdminUser : User
    {
        public string RoleName { get; set; } = null!;             
        public string Department { get; set; } = null!;
        public string? Permissions { get; set; }                  
        public string? LoginIpAddress { get; set; }
    }
}