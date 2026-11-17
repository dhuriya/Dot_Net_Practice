using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EFCoreInheritance.Data;
using EFCoreInheritance.DTOs;
using EFCoreInheritance.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EFCoreInheritance.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentsController : ControllerBase
    {
        private readonly ECommerceDBContext _context;
        public PaymentsController(ECommerceDBContext context)
        {
            _context = context;
        }
        // ---------------- CREATE PAYMENT (Unified) ----------------
        [HttpPost("Create")]
        public async Task<IActionResult> CreatePayment([FromBody] PaymentCreateDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            Payment payment;
            switch (dto.PaymentType.ToLower())
            {
                case "card":
                    if (string.IsNullOrWhiteSpace(dto.CardNumber) ||
                        string.IsNullOrWhiteSpace(dto.CardHolderName) ||
                        !dto.ExpiryMonth.HasValue || !dto.ExpiryYear.HasValue ||
                        string.IsNullOrWhiteSpace(dto.CVV))
                        return BadRequest("Card payment requires CardNumber, CardHolderName, ExpiryMonth, ExpiryYear, and CVV.");
                    payment = new CardPayment
                    {
                        Amount = dto.Amount,
                        Currency = dto.Currency,
                        PaymentGateway = dto.PaymentGateway,
                        OrderId = dto.OrderId,
                        TransactionId = dto.TransactionId,
                        PaymentDate = DateTime.UtcNow,
                        PaymentStatus = PaymentStatus.Pending,
                        CardNumber = dto.CardNumber!,
                        CardHolderName = dto.CardHolderName!,
                        ExpiryMonth = dto.ExpiryMonth.Value,
                        ExpiryYear = dto.ExpiryYear.Value,
                        CVV = dto.CVV!
                    };
                    break;
                case "upi":
                    if (string.IsNullOrWhiteSpace(dto.UpiId))
                        return BadRequest("UPI payment requires UpiId.");
                    payment = new UpiPayment
                    {
                        Amount = dto.Amount,
                        Currency = dto.Currency,
                        PaymentGateway = dto.PaymentGateway,
                        OrderId = dto.OrderId,
                        TransactionId = dto.TransactionId,
                        PaymentDate = DateTime.UtcNow,
                        PaymentStatus = PaymentStatus.Pending,
                        UpiId = dto.UpiId!,
                        AppName = dto.AppName,
                        TransactionRefNo = dto.TransactionRefNo
                    };
                    break;
                case "wallet":
                    if (string.IsNullOrWhiteSpace(dto.WalletType))
                        return BadRequest("Wallet payment requires WalletType.");
                    payment = new WalletPayment
                    {
                        Amount = dto.Amount,
                        Currency = dto.Currency,
                        PaymentGateway = dto.PaymentGateway,
                        OrderId = dto.OrderId,
                        TransactionId = dto.TransactionId,
                        PaymentDate = DateTime.UtcNow,
                        PaymentStatus = PaymentStatus.Pending,
                        WalletType = dto.WalletType!,
                        WalletBalanceUsed = dto.WalletBalanceUsed ?? 0,
                        CashbackReceived = dto.CashbackReceived ?? 0
                    };
                    break;
                default:
                    return BadRequest("Invalid PaymentType. Allowed values: Card, UPI, Wallet.");
            }
            await _context.Payments.AddAsync(payment);
            await _context.SaveChangesAsync();
            return Ok(new
            {
                Message = $"{dto.PaymentType} payment created successfully",
                PaymentId = payment.Id
            });
        }
        // ---------------- GET ALL PAYMENTS ----------------
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllPayments()
        {
            var payments = await _context.Payments.ToListAsync();
            if (payments.Count == 0)
                return NotFound("No payments found.");
            var result = payments.Select(p => new PaymentReadDTO
            {
                Id = p.Id,
                Amount = p.Amount,
                Currency = p.Currency,
                PaymentGateway = p.PaymentGateway,
                PaymentStatus = p.PaymentStatus.ToString(),
                PaymentDate = p.PaymentDate,
                PaymentType = p.GetType().Name
            }).ToList();
            return Ok(result);
        }
        // ---------------- GET PAYMENT BY ID ----------------
        [HttpGet("GetById/{id:int}")]
        public async Task<IActionResult> GetPaymentById(int id)
        {
            var payment = await _context.Payments.FirstOrDefaultAsync(p => p.Id == id);
            if (payment == null)
                return NotFound($"Payment with ID {id} not found.");
            // Optional: Include type-specific data
            object detailed;
            switch (payment)
            {
                case CardPayment card:
                    detailed = new
                    {
                        card.Id,
                        card.Amount,
                        card.Currency,
                        card.PaymentGateway,
                        card.PaymentStatus,
                        card.PaymentDate,
                        PaymentType = "Card",
                        card.CardHolderName,
                        card.ExpiryMonth,
                        card.ExpiryYear,
                        card.MaskedCard
                    };
                    break;
                case UpiPayment upi:
                    detailed = new
                    {
                        upi.Id,
                        upi.Amount,
                        upi.Currency,
                        upi.PaymentGateway,
                        upi.PaymentStatus,
                        upi.PaymentDate,
                        PaymentType = "UPI",
                        upi.UpiId,
                        upi.AppName,
                        upi.TransactionRefNo
                    };
                    break;
                case WalletPayment wallet:
                    detailed = new
                    {
                        wallet.Id,
                        wallet.Amount,
                        wallet.Currency,
                        wallet.PaymentGateway,
                        wallet.PaymentStatus,
                        wallet.PaymentDate,
                        PaymentType = "Wallet",
                        wallet.WalletType,
                        wallet.WalletBalanceUsed,
                        wallet.CashbackReceived
                    };
                    break;
                default:
                    detailed = payment;
                    break;
            }
            return Ok(detailed);
        }
        // ---------------- GET PAYMENTS BY TYPE ----------------
        [HttpGet("GetByType/{type}")]
        public async Task<IActionResult> GetPaymentsByType(string type)
        {
            type = type.ToLower();
            IEnumerable<Payment> payments = type switch
            {
                "card" => await _context.Payments.OfType<CardPayment>().ToListAsync(),
                "upi" => await _context.Payments.OfType<UpiPayment>().ToListAsync(),
                "wallet" => await _context.Payments.OfType<WalletPayment>().ToListAsync(),
                _ => new List<Payment>()
            };
            if (!payments.Any())
                return NotFound($"No {type} payments found.");
            var result = payments.Select(p => new PaymentReadDTO
            {
                Id = p.Id,
                Amount = p.Amount,
                Currency = p.Currency,
                PaymentGateway = p.PaymentGateway,
                PaymentStatus = p.PaymentStatus.ToString(),
                PaymentDate = p.PaymentDate,
                PaymentType = p.GetType().Name
            }).ToList();
            return Ok(result);
        }
    }
}