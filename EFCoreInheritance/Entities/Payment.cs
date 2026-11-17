using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EFCoreInheritance.Entities
{
    public enum PaymentStatus
    {
        Pending = 1,
        Completed = 2,
        Failed = 3,
        Refunded =4
    }
    
    // Base entity (non-abstract, common fields)
    public class Payment
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; } = null!;
        public string PaymentGateway { get; set; } = null!;
        public int OrderId { get; set; }
        public string? TransactionId { get; set; }
        public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Pending;
        public DateTime PaymentDate { get; set; } = DateTime.UtcNow;
        public string? Remarks { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
    // Derived classes
    public class CardPayment : Payment
    {
        public string CardNumber { get; set; } = null!;
        public string CardHolderName { get; set; } = null!;
        public int ExpiryMonth { get; set; }
        public int ExpiryYear { get; set; }
        public string CVV { get; set; } = null!;
        public string MaskedCard => $"XXXX-XXXX-XXXX-{CardNumber[^4..]}";
    }
    public class UpiPayment : Payment
    {
        public string UpiId { get; set; } = null!;
        public string? AppName { get; set; }
        public string? TransactionRefNo { get; set; }
    }
    public class WalletPayment : Payment
    {
        public string WalletType { get; set; } = null!;
        public decimal WalletBalanceUsed { get; set; }
        public decimal CashbackReceived { get; set; }
    }
}