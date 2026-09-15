using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JwtTokenBaseAuthentication.DTO;
using JwtTokenBaseAuthentication.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JwtTokenBaseAuthentication.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        [HttpPost("Create")]
        [Authorize(Roles ="Admin,User")]
        public async Task<IActionResult> Create(ProductRequesetDto productRequesetDto)
        {
            var result = await _productService.Create(productRequesetDto);
            return Ok(result);
        }
        [HttpGet("GetById/{id}")]
        [Authorize(Roles ="Admin,User")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _productService.GetById(id);
            if(result == null)
            {
                return NotFound(new {message = "Product not found"});
            }
            return Ok(result);
        }
        [HttpDelete("Delete/{id}")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _productService.Delete(id);
            if(result == "Product not found")
            {
                return NotFound(new {message = result});
            }
            return Ok(new {message = result});
        }
    }
}