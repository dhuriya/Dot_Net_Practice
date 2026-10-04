using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECommerceApp.Data;
using ECommerceApp.DTOs;
using ECommerceApp.Enums;
using ECommerceApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ECommerceApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly AppDbContext _context;
        public OrderController(AppDbContext context)
        {
            _context = context;
        }
        [HttpPost("place")]
        public async Task<IActionResult> PlaceOrder(OrderRequestDTO requestDTO)
        {
            try
            {
                var customer = await _context.Customers.Include(c => c.Addresses)
                    .FirstOrDefaultAsync(c => c.CustomerId == requestDTO.CustomerId && c.IsActive);
                if (customer == null)
                {
                    return BadRequest(new { Message = "Invalid CustomerId or customer is not active." });
                }
                var shipping = await _context.Addresses
                    .FirstOrDefaultAsync(a => a.AddressId == requestDTO.ShippingAddressId 
                    && a.CustomerId == requestDTO.CustomerId);
                var billing = await _context.Addresses
                    .FirstOrDefaultAsync(a => a.AddressId == requestDTO.BillingAddressId 
                    && a.CustomerId == requestDTO.CustomerId);
                if (shipping == null || billing == null)
                    return BadRequest("Invalid Shipping or Billing Address for this customer.");
                    
                var productIds = requestDTO.OrderItems.Select(oi => oi.ProductId).ToList();
                var products = await _context.Products
                    .Where(p => productIds.Contains(p.ProductId) && p.IsActive)
                    .ToListAsync();
                if(products.Count != productIds.Count)
                {
                    return BadRequest(new { Message = "One or more ProductIds are invalid or inactive." });
                }
                foreach(var item in requestDTO.OrderItems)
                {
                    var product = products.FirstOrDefault(p => p.ProductId == item.ProductId);
                    if(product.Stock < item.Quantity)
                    {
                        return BadRequest($"Insufficient stock for '{product.Name}'. Available: {product.Stock}, Requested: {item.Quantity}");
                    }
                }
                var order = new Order
                {
                    CustomerId = requestDTO.CustomerId,
                    ShippingAddressId = requestDTO.ShippingAddressId,
                    BillingAddressId = requestDTO.BillingAddressId,
                    OrderStatusId = (int)OrderStatusEnum.Pending,
                    OrderDate = DateTime.UtcNow,
                    CreateAt = DateTime.UtcNow,
                    CreatedBy = "System", // This should ideally come from the authenticated user context
                    IsActive = true
                };
                //  Map Order Items and Deduct Stock
                order.OrderItems = new List<OrderItem>();
                foreach(var items in requestDTO.OrderItems)
                {
                    var product = products.First(p => p.ProductId == items.ProductId);
                }
            }
        }
    }
}