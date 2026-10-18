using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECommerce.App.Data;
using ECommerce.App.Domain;
using ECommerce.App.DTOs;
using ECommerce.App.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.App.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly ECommerceDBContext _context;
        public OrdersController(ECommerceDBContext context)
        {
            _context = context;
        }
        [HttpPost("place")]
        public async Task<IActionResult> PlaceOrder([FromBody] OrderRequestDTO request)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            var customer = await _context.Customers.
            FirstOrDefaultAsync(c => c.Id == request.CustomerId && !c.IsDeleted);
            if(customer == null)
                return BadRequest("Customer does not exist oris inactive.");
            if(request.Items == null || !request.Items.Any())
                return BadRequest("Order must contain at least one item.");
            decimal totalAmount = 0;
            var orderItems = new List<OrderItem>();
            foreach(var item in request.Items)
            {
                var product = await _context.Products.
                FirstOrDefaultAsync(p => p.Id == item.ProductId && !p.IsDeleted);
                if(product == null)
                return BadRequest($"productId {item.ProductId} does not exist.");

                if(item.Quantity <=0)
                    return BadRequest($"Invalid quantity for Productit {item.ProductId}. Qunatity must be at least 1.");
                
                if(product.Quantity < item.Quantity)
                    return BadRequest($"Insufficient stock for product: {product.Name}. Available: {product.Quantity}");

                product.Quantity -= item.Quantity;
                var lineTotal = product.Price * product.Quantity;
                totalAmount += lineTotal;
                orderItems.Add(new OrderItem
                {
                    ProductId = product.Id,
                    Quantity = product.Quantity,
                    LineTotal = lineTotal
                });

            }
            var order = new Order
                {
                    Id = request.CustomerId,
                    TotalAmount = totalAmount,
                    OrderStatus = OrderStatus.Pending,
                    PaymentStatus = PaymentStatus.Pending,
                    IsDeleted = false,
                    OrderItem = orderItems
                };
                _context.Orders.Add(order);
                await _context.SaveChangesAsync();
                // Return success response
                return Ok(new
                {
                    Message = "Order placed successfully.",
                    OrderId = order.Id,
                    TotalAmount = order.TotalAmount,
                    Items = orderItems.Count
                });
        }
    }
}