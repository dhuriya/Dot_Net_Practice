using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MapperlyOrderDemo.DTOs;
using MapperlyOrderDemo.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;

namespace MapperlyOrderDemo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _service;
        private readonly ILogger<CustomerController> _logger;
        public CustomerController(ICustomerService service, ILogger<CustomerController> logger)
        {
            _service = service;
            _logger = logger;
        }
        // POST: api/customers
        [HttpPost]
        [ProducesResponseType(typeof(CustomerResponseDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateCustomerRequestDTO dto)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);
            try
            {
                var created = await _service.CreateAsync(dto);
                // Location header points to GET api/customers/{id}
                return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Customer create failed.");
                return BadRequest(new { message = ex.Message });
            }
        }
        // GET: api/customers
        [HttpGet]
        [ProducesResponseType(typeof(List<CustomerResponseDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllActive()
        {
            try
            {
                var customers = await _service.GetAllActiveAsync();
                return Ok(customers);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex,"Fetching customers failed");
                 return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Something went wrong while fetching customers." });
            }
        }
        // GET: api/customers/{id}
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(CustomerResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            try
            {
                var customer = await _service.GetByIdAsync(id);
                if(customer == null)
                    return NotFound(new {message = "Customer not found, Id = {id}"});
                return Ok(customer);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Fetching customer failed. CustomerId={CustomerId}", id);
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new { message = "Something went wrong while fetching the customer." });
            }
        }
    }
}