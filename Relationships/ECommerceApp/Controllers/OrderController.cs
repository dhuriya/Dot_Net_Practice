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
        public async Task<IActionResult> PlaceOrder([FromBody]OrderRequestDTO requestDTO)
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
                    if(product?.Stock < item.Quantity)
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
                    product.Stock -= items.Quantity;
                    product.UpdatedAt = DateTime.UtcNow;
                    product.UpdatedBy = "System";
                    //Create order item record
                    order.OrderItems.Add(new OrderItem
                    {
                       ProductId = product.ProductId,
                       Quantity = items.Quantity,
                       UnitPrice = product.Price 
                    });
                }
                order.TotalAmount = order.OrderItems.Sum(i => i.Quantity * i.UnitPrice);
                _context.Orders.Add(order);
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    Message = "Order placed Successfully.",
                    OrderId = order.OrderId
                });
            }catch(Exception ex)
            {
                return StatusCode(500, new
                {
                    Message = "Unexpected error while placing the order.",
                    ErrorMessage = ex.Message
                });
            }
        }
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<OrderResponseDTO>>> GetAllOrders()
        {
            
            try
            {
                // Step 1 : Query orders and eagerly load all relationships
                // .Include() loads one related enity.
                // .ThenInclude() allows chaining into deeper levels of relationships.
                // .AsNoTracking() impproves performance for read-only data (no change tracking)
                var orders = await _context.Orders.AsNoTracking().
                Include(o => o.Customer).ThenInclude(c=>c.Profile)// Order -> customer -> profile
                .Include(o=>o.Customer).ThenInclude(c =>c.Addresses)// Order-> customer -> Addresses
                .Include(o=>o.OrderItems).ThenInclude(oi=> oi.Product)// Order -> Orderitems -> Products
                .ThenInclude(p=>p.Category)// Product -> Category (multi-level)
                .Include(o =>o.OrderStatus)// Order -> OrderStatus
                .Include(o=>o.ShippingAddress)//Order -> ShippingAddress
                .Include(o=>o.BillingAddress)// Order-> BillingAddress
                .Where(o => o.IsActive)// Fetch only active orders
                .ToListAsync();
                // Step2 : Mapp entites to DTOs.
                // DTO ensure that the API only exposes strctured, safe, non-circular data.
                var result = orders.Select(o => new OrderResponseDTO
                {
                    OrderId = o.OrderId,
                    OrderDate = o.OrderDate.ToString("yyyy-MM-dd HH:mm:ss"),
                    Status = o.OrderStatus.Name,
                    TotalAmount = o.TotalAmount,
                    ShippingAddress = $"{o.ShippingAddress?.Line1}, {o.ShippingAddress?.City},{o.ShippingAddress?.Country}",
                    BillingAddress = $"{o.BillingAddress?.Line1},{o.BillingAddress?.City},{o.BillingAddress?.Country}",
                    //Nested DTO representing Customer + Profile relationship.
                    Customer = new CustomerResponseDTO
                    {
                        CustomerId = o.Customer.CustomerId,
                        Name = o.Customer.Name,
                        Email = o.Customer.Email,
                        Phone = o.Customer.Phone,
                        DisplayName = o.Customer.Profile?.DisplayName,
                        Gender = o.Customer.Profile?.Gender,
                        DateOfBirth =o.Customer.Profile?.DateOfBirth.ToString("yyyy-MM-dd")
                    },
                    //  Colleaction mapping for Order -> OrderItems -> Product -> Category
                    Items = o.OrderItems?.Select(i => new OrderItemResponseDTO
                    {
                        ProductName = i.Product.Name,
                        Category = i.Product.Category.Name,
                        Quantity = i.Quantity,
                        UnitPrice = i.UnitPrice
                    }).ToList() ?? new List<OrderItemResponseDTO>()
                });
                // Step 3 return the list or map DTOs/
                // At this point all related data has been loaded in a single query.
                return Ok(result);
            }
            catch (Exception ex)
            {
                
                return StatusCode(500 , new
                {
                    Message = "An error occurred while fetching orders",
                    ErrorMessage = ex.Message
                });
            }
        }
        [HttpGet("{id:int}")]
        public async Task<ActionResult<OrderResponseDTO>> GetOrderById(int id)
        {
            try
            {
                // Step 1 : Query the target order list id, including all necessary navigation
                var o = await _context.Orders.AsNoTracking()
                .Include(o => o.Customer).ThenInclude(c => c.Profile)
                // One-to-One : Customer -> Profile
                .Include(o => o.Customer).ThenInclude(c => c.Addresses)
                // One-to-Many: custome -> Addresses
                .Include(o => o.OrderItems).ThenInclude(oi=>oi.Product)
                // One-to-Many : Order-> Orderitems
                .ThenInclude(p => p.Category)//Many-to - one : Product -> Category
                .Include(o =>o.OrderStatus).Include(o=>o.ShippingAddress)
                .Include(o=>o.BillingAddress).FirstOrDefaultAsync(o => o.OrderId == id && o.IsActive);
                // Step 2 : Handle not found scenario
                if(o == null)
                {
                    return NotFound($"Order with {id} not found");
                }
                // Step 3: Map the entity and item related data to a DTO.
                // This keeps response shape predictable and serialization -safe.
                var response = new OrderResponseDTO
                {
                    OrderId = o.OrderId,
                    OrderDate = o.OrderDate.ToString("yyyy-MM-dd HH:mm:ss"),
                    Status = o.OrderStatus.Name,
                    TotalAmount = o.TotalAmount,
                    ShippingAddress = $"{o.ShippingAddress?.Line1},{o.ShippingAddress?.City},{o.ShippingAddress?.Country}",
                    BillingAddress = $"{o.BillingAddress?.Line1},{o.BillingAddress?.City},{o.BillingAddress?.Country}",
                    Customer = new CustomerResponseDTO
                    {
                        CustomerId = o.Customer.CustomerId,
                        Name = o.Customer.Name,
                        Email = o.Customer.Email,
                        Phone = o.Customer.Phone,
                        DisplayName = o.Customer.Profile?.DisplayName,
                        Gender = o.Customer.Profile?.Gender,
                        DateOfBirth = o.Customer.Profile?.DateOfBirth.ToString("yyyy-MM-dd")
                    },
                    Items = o.OrderItems?.Select(i => new OrderItemResponseDTO
                    {
                        ProductName = i.Product.Name,
                        Category = i.Product.Category.Name,
                        Quantity = i.Quantity,
                        UnitPrice = i.UnitPrice
                    }).ToList() ?? new List<OrderItemResponseDTO>()
                };
                return Ok(response);
            }
            catch(Exception ex)
            {
                return StatusCode(500, new
                {
                    Message = "An Error Occurred while fetching orders.",
                    ErrorMessage = ex.Message
                });
            }
        }
        [HttpGet("filtered-orders")]
        public async Task<IActionResult> GetFilteredOrders([FromQuery] int days = 30, [FromQuery] decimal minAmount = 1000)
        {
            var query = _context.Orders.Where(o => o.IsActive && o.OrderDate >=
            DateTime.Now.AddDays(-days) && o.TotalAmount >= minAmount && 
            o.Customer.IsActive).AsNoTracking().
            Include(o => o.Customer).ThenInclude(c => c.Profile).
            Include(o => o.OrderItems.Where(i => i.Product.IsActive)).
            ThenInclude(i => i.Product).
            ThenInclude(p => p.Category).
            Include(o => o.BillingAddress).
            Include(o => o.ShippingAddress);
            var orders = await query.ToListAsync();
            if(!orders.Any())
            {
                return Ok(new
                {
                    Message = "No matching orders found for the given criteria.",
                    TotalOrders = 0
                });
            }
            var result = orders.Select(o => new
            {
                o.OrderId,
                o.OrderDate,
                o.TotalAmount,
                Customer = o.Customer.Name,
                Profile = o.Customer.Profile?.DisplayName,
                Cities = o.Customer.Addresses?.Select(a => a.City).ToList(),
                ProductNames = o.OrderItems.Select(i => i.Product.Name).ToList(),
                ItemCount = o.OrderItems.Count
            });
            return Ok(new
            {
                Message = $"Filltered eager loadig executed successfully (last {days}), min {minAmount}",
                TotalOrders = result.Count(),
                SampleResult = result.Take(3)
            });
        }
        [HttpGet("explicit/{id:int}")]
        public async Task<IActionResult> GetOrderWithExplicitLoadig(int id)
        {
            // Step 1: Load only the main entity first
            var order = await _context.Orders.
            AsNoTracking().FirstOrDefaultAsync(o => o.OrderId == id);
            if(order == null)
                return NotFound($"Order with ID {id} not found");
            //Step 2 : related entities explicity when required.
            // Load customer reference
            await _context.Entry(order).Reference(o => o.Customer).LoadAsync();
            // Load OrderItems collection
            await _context.Entry(order).Collection(o => o.OrderItems).LoadAsync();
            // Load OrderStatus (single related entity)
            await _context.Entry(order).Reference(o => o.OrderStatus).LoadAsync();
            // Load nested relationship: OrderItems → Product → Category
            foreach (var item in order.OrderItems)
            {
                await _context.Entry(item).Reference(i => i.Product).LoadAsync();
                await _context.Entry(item.Product).Reference(p => p.Category)
                .LoadAsync();
            }
            // STEP 3: Construct DTO manually (avoiding circular references)
            var response = new {
                order.OrderId,
                order.OrderDate,
                order.TotalAmount,
                Status = order.OrderStatus?.Name,
                customer = order.Customer == null ? null : new {
                    order.customer.CustomerId,
                    order.customer.Name,
                    order.Customer.Email,
                    order.customer.Phone
                },
                Items = order.OrderItems?.Select(i => new
                {
                    i.ProductId,
                    ProductName = i.Product?.Name,
                    Category = i.Product?.Category?.Name,
                    i.Quantity,
                    i.UnitPrice
                })
            };
            return Ok(response);
        }
        [HttpGet("lazy-highvalue")]
        public IActionResult GetHighValueOrdersLazy()
        {
            // STEP 1: Load all active orders placed in the last 30 days.
            // This executes a SINGLE SQL query to fetch Orders only.
            // Related entities (Customer, OrderItems, Product) are NOT loaded yet.
            var orders = _context.Orders
                                 .Where(o => o.IsActive && o.OrderDate >= DateTime.Now.AddDays(-30))
                                 .ToList();
            var result = new List<object>();
            // =====================================================
            // STEP 2: Iterate through each Order and access navigation properties.
            // IMPORTANT: At this point, Lazy Loading kicks in automatically.
            // EF Core dynamically generates "proxy" objects for these entities.
            // When you access a navigation property (like order.Customer or order.OrderItems),
            // EF Core INTERCEPTS the property access and issues a NEW SQL query behind the scenes.
            //
            // This is where the "N+1 Query Problem" occurs:
            //     - 1 query to load all Orders
            //     - +N queries for each Order’s Customer
            //     - +N more queries for each Order’s OrderItems
            //     - +extra queries if OrderItems reference Products
            //   Resulting in dozens or hundreds of hidden queries.
            // =====================================================
            foreach (var order in orders)
            {
                // Accessing this property fires an automatic SELECT query like:
                // SELECT * FROM Customers WHERE CustomerId = @Order.CustomerId
                var customer = order.Customer; // Triggers a separate SQL query per order.
                // Accessing this property also triggers a new SELECT query like:
                // SELECT * FROM OrderItems WHERE OrderId = @Order.OrderId
                // (and additional queries if Lazy Loading is also enabled for Product)
                var items = order.OrderItems; // Another separate SQL query per order.
                // We want to find only:
                //   - Orders with Active Customers
                //   - OrderItems with Quantity ≥ 3 and Product.IsActive = true
                // But Lazy Loading already fetched ALL data for every order.
                // So the filtering below happens in MEMORY, not in SQL.
                // This means unnecessary rows are fetched from the database.
                var validItems = items?
                    .Where(i => i.Quantity >= 3 && i.Product.IsActive)
                    .ToList(); // Filter applied in memory, not at DB level.
                // Even though we check here for active customers,
                // the Customer entity was already fully loaded by EF Core earlier.
                // There’s no way to prevent EF from loading all customers upfront.
                if (customer != null && customer.IsActive && validItems?.Any() == true)
                {
                    result.Add(new
                    {
                        order.OrderId,
                        CustomerName = customer.Name,
                        FilteredItemCount = validItems.Count,
                        TotalAmount = order.TotalAmount
                    });
                }
            }
            // ======================================================
            // STEP 3: Return the results.
            // The API returns only filtered data, but the real issue is PERFORMANCE:
            //   - EF Core already fired multiple hidden queries.
            //   - Filtering was done in memory instead of in SQL.
            //   - This approach does not scale for large datasets.
            // ======================================================
            return Ok(new
            {
                Message = "Lazy Loading fetched *all* related data, even those not needed. Filtering happened in memory.",
                TotalOrders = result.Count,
                SampleResult = result.Take(3)
            });
        }
        [HttpGet("explicit-highvalue")]
        public async Task<IActionResult> GetHighValueOrdersExplicit()
        {
            // =====================================================
            // STEP 1: Load the base entities (Orders)
            // This executes ONE SQL query that fetches all active orders placed in the last 30 days.
            // Note:
            //   - No related data (Customer, OrderItems, Product) is loaded yet.
            //   - We use AsNoTracking() for performance since we’re only reading data.
            var orders = await _context.Orders
                                       .Where(o => o.IsActive && o.OrderDate >= DateTime.Now.AddDays(-30))
                                       .AsNoTracking()
                                       .ToListAsync();
            if (!orders.Any())
            {
                // If no orders found, return early to avoid extra database queries.
                return NotFound("No active orders found in the last 30 days.");
            }
            // =======================================================
            // STEP 2: Load only ACTIVE customers for these orders
            // Lazy Loading would have fetched *every* customer (active or inactive) per order automatically.
            // Here, we explicitly select only customers who are active, using a single optimized SQL query.
            var activeCustomerIds = orders
                                    .Select(o => o.CustomerId)
                                    .Distinct()
                                    .ToList();
            var activeCustomers = await _context.Customers
                                                .Where(c => activeCustomerIds.Contains(c.CustomerId) && c.IsActive)
                                                .AsNoTracking()
                                                .ToListAsync();
            // ========================================================
            // STEP 3: Manually attach the loaded Customer objects to each Order
            // This step binds the customer from our manually loaded list to the corresponding Order.
            // Since we filtered customers at the SQL level, we know every Customer here is already active.
            //
            // Note:
            //   - The null-forgiving operator (!) is used because we know each Order has a valid CustomerId.
            //   - Unlike Lazy Loading, no hidden queries are triggered here — it's a pure in-memory assignment.
            foreach (var order in orders)
            {
                order.Customer = activeCustomers.FirstOrDefault(c => c.CustomerId == order.CustomerId)!;
            }
            // ========================================================
            // STEP 4: Explicitly load only required OrderItems
            // Instead of EF Core automatically fetching ALL OrderItems (like Lazy Loading does),
            // we can apply conditional SQL filters directly here using the .Query() method.
            //
            // Filters:
            //   - Include only items where Quantity ≥ 3
            //   - Include only items whose Product is Active
            //
            // The .Include(i => i.Product) ensures that the Product entity is eagerly loaded together
            // for each OrderItem in the same query.
            //
            // This ensures that only the relevant OrderItems and Products are fetched from the database.
            // Each LoadAsync() here executes one precise, optimized SQL query per order.
            // =========================================================
            foreach (var order in orders)
            {
                await _context.Entry(order)
                    .Collection(o => o.OrderItems)   // Access the OrderItems navigation property
                    .Query()                         // Begin building a query for this collection
                    .Where(i => i.Quantity >= 3 && i.Product.IsActive)  // Apply conditional SQL filter
                    .Include(i => i.Product)          // Include related Product data
                    .LoadAsync();                     // Execute query explicitly
            }
            // ==========================================================
            // STEP 5: Build the final filtered result for output
            // Now that we’ve manually controlled what was loaded:
            //   - Each Order contains only Active Customers.
            //   - Each Order’s OrderItems collection contains only products meeting our filter criteria.
            //
            // The .Where() here ensures we only project orders that meet both relational conditions.
            var filteredOrders = orders
                .Where(o => o.Customer != null && o.OrderItems.Any()) // Keep only valid, filtered orders
                .Select(o => new
                {
                    o.OrderId,
                    CustomerName = o.Customer!.Name,
                    FilteredItemCount = o.OrderItems.Count,
                    TotalAmount = o.TotalAmount
                })
                .ToList();
            // ==========================================================
            // STEP 6: Handle case where no orders matched after filtering
            // If no Order met the conditions (active customer, min quantity, active products),
            // return an appropriate response message.
            if (!filteredOrders.Any())
            {
                return Ok(new
                {
                    Message = "No matching orders found after applying conditional filters.",
                    TotalOrders = 0
                });
            }
            // =========================================================
            // STEP 7: Return the final result
            // This approach ensures:
            //   - Minimal and controlled SQL queries
            //   - SQL-level filtering (not in-memory)
            //   - No N+1 problem
            //   - Full transparency and performance predictability
            //
            // Summary of executed queries:
            //   1️ Orders (base query)
            //   2️ Customers (filtered by active)
            //   3️ OrderItems (filtered by Quantity & Product status, per order)
            return Ok(new
            {
                Message = "Explicit Loading with precise filters. Only Active Customers and Products (Quantity ≥ 3) fetched.",
                TotalOrders = filteredOrders.Count,
                SampleResult = filteredOrders.Take(3)
            });
        }
    }
}