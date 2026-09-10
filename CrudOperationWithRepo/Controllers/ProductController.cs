using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CrudOperationWithRepo.Services.IService;
using Microsoft.AspNetCore.Mvc;

namespace CrudOperationWithRepo.Controllers
{
    // what is the dependency injection in C#?
    // Dependency Injection (DI) is a design pattern that allows us to achieve loose coupling between classes and their 
    // dependencies by providing the required dependencies from outside the class instead of creating them inside the class.

    // Dependency Injection means rather then writing all the code in the same controller we can call from the outside
    // or services layer.
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productSerivce;
        public ProductController(IProductService productService)
        {
            _productSerivce = productService;
        }
        // Here we also follow the Command and Query Separation (CQS) architecture, which states that a method should 
        // either be a command that performs an action or a query that returns data, but it should not do both at the 
        // same time.
        //GET: api/Product/GetAll
        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            var proudcts = _productSerivce.GetAllProucts();
            return Ok(proudcts);
        }
        //GET: api/Product/GetById/1
        [HttpGet("GetById/{id}")]
        public IActionResult GetById(int id)
        {
            var product = _productSerivce.GetById(id);
            if(product == null)
            {
                return NotFound("Product Not Found");
            }
            return Ok(product);
        }
        //POST: api/Product/Add
        [HttpPost("Add")]
        public IActionResult Create(Product product)
        {
            if(product == null)
            {
                return BadRequest("Product is null");
            }
            _productSerivce.Create(product);
            return Ok("Product Added succesfully");
        }
        //PUT: api/Product/Update/1
        [HttpPut("Update/{id}")]
        public IActionResult Update(int id, Product product)
        {
            if(product == null)
            {
                return BadRequest("Invalid Proudct data");
            }
            _productSerivce.Update(product);
            return Ok("Product Updated Successfully");
        }
        //DELETE: api/Product/Delete/Id
        [HttpDelete("Delete/{id}")]
        public IActionResult Delete(int id)
        {
            _productSerivce.Delete(id);
            return Ok("Product Delete Successfully");
        }
    }
}