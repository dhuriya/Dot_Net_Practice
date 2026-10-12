using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MapperlyDemo.DTOs;
using MapperlyDemo.Services;
using Microsoft.AspNetCore.Mvc;

namespace MapperlyDemo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _service;
        public ProductController(IProductService service)
        {
            _service = service;
        }
        [HttpGet]
        public async Task<ActionResult<List<ProductResponseDTO>>> GetAll()
        {
            var products = await _service.GetAllAsync();
            return Ok(products);
        }
        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProductResponseDTO>> GetById(int id)
        {
            var product = await _service.GetByIdAsync(id);
            if(product is null)
                return NotFound();
            return Ok(product);
        }
        [HttpPost]
        public async Task<ActionResult<ProductResponseDTO>> Create([FromBody] CreateProductRequestDTO dto)
        {
            var created = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new {id = created.Id}, created);
        }
        [HttpPut]
        public async Task<ActionResult<ProductResponseDTO>> Update([FromBody] UpdateProductRequestDTO dto)
        {
            var updated = await _service.UpdateAsync(dto);
            if(updated is null)
                return NotFound();
            return Ok(updated);
        }
        
    }
}