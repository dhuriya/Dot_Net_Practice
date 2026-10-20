using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MapperlyOrderDemo.DTOs;
using MapperlyOrderDemo.Services;
using Microsoft.AspNetCore.Mvc;

namespace MapperlyOrderDemo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _service;
        private readonly ILogger<OrderController> _logger;
        public OrderController(IOrderService service, ILogger<OrderController> logger)
        {
            _service = service;
            _logger = logger;
        }
        // POST: api/orders
        [HttpPost]
        [ProducesResponseType(typeof(OrderResponseDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateOrderRequestDTO dto)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);
            try
            {
                var created = await _service.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { orderId = created.Id }, created);
            }catch(Exception ex)
            {
                _logger.LogError(ex, "Order create failed.");
                return BadRequest(new { message = ex.Message });
            }
        }
        [HttpGet("{orderId:int}")]
        [ProducesResponseType(typeof(OrderResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById([FromRoute] int orderId)
        {
            try
            {
                var order = await _service.GetByIdAsync(orderId);
                if(order == null)
                    return NotFound(new { message = $"Order not found. OrderId={orderId}" });
                return Ok(order);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Fetching order failed. OrderId={OrderId}", orderId);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Something went wrong while fetching the order." });
            }
        }
        // GET: api/orders/by-customer/{customerId}
        [HttpGet("by-customer/{customerId:int}")]
        [ProducesResponseType(typeof(List<OrderResponseDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByCustomer([FromRoute] int customerId)
        {
            try
            {
                var orders = await _service.GetByCustomerAsync(customerId);
                return Ok(orders);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fetching orders failed. CustomerId={CustomerId}", customerId);
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}