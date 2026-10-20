using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MapperlyOrderDemo.Data;
using MapperlyOrderDemo.DTOs;
using MapperlyOrderDemo.Entities;
using MapperlyOrderDemo.Mappers;
using Microsoft.EntityFrameworkCore;

namespace MapperlyOrderDemo.Services
{
    public class OrderService : IOrderService
    {
        // DbContext for database access
        private readonly AppDbContext _dbContext;
        //Mapperly-generated mapper for order
        private readonly OrderMapper _mapper;

        // Logger for logging
        private readonly ILogger<OrderService> _logger;

        public OrderService(AppDbContext appDbContext, OrderMapper mapper, ILogger<OrderService> logger)
        {
            _dbContext = appDbContext;
            _mapper = mapper;
            _logger = logger;
        }
        // CREATE ORDER
        // Input  : CreateOrderRequestDTO (from API)
        // Output : OrderResponseDTO (to API)
        //
        // High-level flow:
        // 1. Validate request-level rules
        // 2. Validate customer existence
        // 3. Map DTO -> Order entity (base mapping)
        // 4. Load and validate products
        // 5. Build order items and compute total
        // 6. Set system/business fields
        // 7. Persist order
        // 8. Build response DTO
        public async Task<OrderResponseDTO> CreateAsync(CreateOrderRequestDTO dto)
        {

            try
            {
                _logger.LogInformation("Creating order. CustomerId = {CustomerId}", dto.CustomerId);
                // Basic request-level validation
                // Ensure at least one item is present
                if(dto.Items == null || dto.Items.Count == 0)
                    throw new InvalidOperationException("At least one order item is required.");
                // Ensure all quantities are positive
                if(dto.Items.Any(i => i.Quantity <= 0))
                    throw new InvalidOperationException(
                        "Quantity must be greater than zero.");
                // Validate customer existence and status
                // Orders can be placed only by active customers
                var customer = await _dbContext.Customers.
                FirstOrDefaultAsync(c => c.Id == dto.CustomerId && c.IsActive);
                if(customer == null)
                    throw new InvalidOperationException("Customer does not exist or is inactive.");
                // Base mapping: DTO -> Order entity
                // Mapperly maps only simple/structural fields here.
                var order = _mapper.ToEntity(dto);
                // Assign customer relationship explicitly
                order.CustomerId = dto.CustomerId;
                order.Customer =customer;
                // Load and validate products
                // Fetch all required products in ONE database call
                var productIds = dto.Items.
                Select(i => i.ProductId).Distinct().ToList();
                var products = await _dbContext.Products
                .Where(p => productIds.Contains(p.Id) && p.IsActive)
                .ToDictionaryAsync(p => p.Id);

                if(productIds.Count != products.Count)
                    throw new InvalidOperationException("Some products do not exist or are inactive");
                
                // Build order items and calculate total amount
                decimal total = 0m;
                foreach(var itemDto in dto.Items)
                {
                    var product = products[itemDto.ProductId];
                    // Map item DTO -> OrderItem entity
                    var orderItem = _mapper.ToEntity(itemDto);
                    // Assign product relationship
                    // orderItem.ProductId = product.Id;
                    orderItem.UnitPrice = product.Price;
                    orderItem.Product = product;
                    order.Items.Add(orderItem);
                    total += product.Price * orderItem.Quantity;
                }
                // System and business fields
                // These values depend on runtime/business rules,
                // so they must NOT be handled by the mapper.
                order.TotalAmount = total;
                order.IsPain = true;
                order.Status = OrderStatus.Paid;
                order.CreatedOn = DateTime.UtcNow;
                order.CreatedBy = "SYSTEM";
                // Persist order
                _dbContext.Orders.Add(order);
                await _dbContext.SaveChangesAsync();
                _logger.LogInformation(
                    "Order created successfully. OrderId={OrderId}, Total={Total}",
                    order.Id,
                    order.TotalAmount);
                // Build and return response DTO
                return BuildOrderResponse(order);
            }
            catch (Exception ex)
            {
                // Log full exception details for diagnostics
                _logger.LogError(
                    ex,
                    "Order creation failed. CustomerId={CustomerId}",
                    dto.CustomerId);
                // Re-throw so controller/middleware can decide HTTP response
                throw;
            }
        }

        // GET ORDERS BY CUSTOMER
        public async Task<List<OrderResponseDTO>> GetByCustomerAsync(int customerId)
        {
            try
            {
                _logger.LogInformation(
                    "Fetching orders for customer. CustomerId={CustomerId}",
                    customerId);
                // Ensure customer exists and is active
                var active = await _dbContext.Customers
                    .AnyAsync(c => c.Id == customerId && c.IsActive);
                if (!active)
                    throw new InvalidOperationException(
                        "Customer does not exist or is inactive.");
                // Load orders with required navigation properties
                var orders = await _dbContext.Orders
                    .Include(o => o.Customer)
                    .Include(o => o.Items)
                        .ThenInclude(i => i.Product)
                    .Where(o => o.CustomerId == customerId)
                    .OrderByDescending(o => o.CreatedOn)
                    .ToListAsync();
                // Build response DTOs
                return orders
                    .Select(BuildOrderResponse)
                    .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Fetch customer orders failed. CustomerId={CustomerId}",
                    customerId);
                throw;
            }
        }

        // GET ORDER BY ID
        public async Task<OrderResponseDTO?> GetByIdAsync(int orderId)
        {
            try
            {
                _logger.LogInformation(
                    "Fetching order. OrderId={OrderId}",
                    orderId);
                // Load order with required navigation properties
                var order = await _dbContext.Orders
                    .Include(o => o.Customer)
                    .Include(o => o.Items)
                        .ThenInclude(i => i.Product)
                    .FirstOrDefaultAsync(o => o.Id == orderId);
                // Return null if order does not exist
                if (order == null)
                    return null;
                return BuildOrderResponse(order);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Fetch order failed. OrderId={OrderId}",
                    orderId);
                throw;
            }
        }
        // PRIVATE HELPER: BUILD ORDER RESPONSE DTO
        private OrderResponseDTO BuildOrderResponse(Order order)
        {
            // Base mapping: Order -> OrderResponseDTO
            var dto = _mapper.ToDto(order);
            //foreach (var item in order.Items) 
            //{
            //    dto.Items.Add(_mapper.ToDto(item));
            //}
            // Map each OrderItem -> OrderItemResponseDTO explicitly
            //dto.Items = order.Items
            //    .Select(i => _mapper.ToDto(i))
            //    .ToList();
            return dto;
        }
    }
}