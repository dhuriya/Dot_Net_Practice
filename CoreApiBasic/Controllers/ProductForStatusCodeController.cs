using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreApiBasic.Models;
using CoreApiBasic.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;

namespace CoreApiBasic.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductForStatusCodeController : ControllerBase
    {
        [HttpGet("GetProductCount")]
        // public async Task<int> GetProductCount()
        // {
        //     return await ProductServiceForStatusCode.GetProductCountAsync();
        // }
        // public async Task<IActionResult> GetProductCount()
        // {
        //     int count = await ProductServiceForStatusCode.GetProductCountAsync();
        //     return Ok(count);
        // }
        public async Task<ActionResult<int>> GetProductCount()
        {
            int count = await ProductServiceForStatusCode.GetProductCountAsync();
            return Ok(count);
        }

        [HttpGet("ProductGetById/{id}")]
        // public async Task<Product> GetProductById(int id)
        // {
        //     return await ProductServiceForStatusCode.GetProductByIdAsync(id);
        // }
        // public async Task<IActionResult> GetProductById(int id)
        // {
        //     if(id <=0)
        //     {
        //          return BadRequest("Invalid Product ID. Must be greater than zero.");
        //     }
        //     var product = await ProductServiceForStatusCode.GetProductByIdAsync(id);
        //     if(product == null)
        //     {
        //         return NotFound($"Product with ID = {id} not found.");
        //     }
        //     return Ok(product);
        // }
        public async Task<ActionResult<Product>> GetProductById(int id)
        {
            if(id<=0)
            {
                return BadRequest("Invalid Product ID. Must be greater than zero.");
            }
            var product = await ProductServiceForStatusCode.GetProductByIdAsync(id);
            if(product == null)
            {
                return NotFound($"Product with ID = {id} not found.");
            }
            return Ok(product);
        }
        [HttpGet("GetAllProduct")]
        // public async Task<List<Product>> GetAllProduct()
        // {
        //     return await ProductServiceForStatusCode.GetAllProductAsync();
        // }
        // public async Task<IActionResult> GetAllProduct()
        // {
        //     var products = await ProductServiceForStatusCode.GetAllProductAsync();
        //     if(products == null || products.Count == 0)
        //     {
        //         return NoContent();//204
        //     }
        //     return Ok(products);
        // }
        public async Task<ActionResult<List<Product>>> GetAllProduct()
        {
            var products = await ProductServiceForStatusCode.GetAllProductAsync();
            if(products == null || product.count == 0)
            {
                return NoContent();
            }
            return Ok(products);
        }
        [HttpGet("GetAllNames")]
        // public async Task<List<string>> GetAllNames()
        // {
        //     return await ProductServiceForStatusCode.GetProductAllNameAsync();
        // }
        // public async Task<IActionResult> GetAllNames()
        // {
        //     var names = await ProductServiceForStatusCode.GetProductAllNameAsync();
        //     if(names == null || names.Count == 0)
        //     {
        //         return NotFound("No product names found.");
        //     }
        //     return Ok(names);
        // }
        public async Task<ActionResult<List<string>>> GetAllNames()
        {
            var names = await ProductServiceForStatusCode.GetProductAllNameAsync();
            if(names == null || names.Count == 0)
            {
                return NotFound("No product names found");
            }
            return Ok(names);
        }
    }
}