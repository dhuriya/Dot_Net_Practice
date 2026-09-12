using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CrudOperationWithDto.DTO.Request;
using CrudOperationWithDto.Interface;
using Microsoft.AspNetCore.Mvc;

namespace CrudOperationWithDto.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        // Dependency Injection means rather then writing all the code in the controller we can write the code in service class
        // and we can call that service class in controller by using dependency injection

        // Abstraction means we can hide the implementation details and only show the functionality to the user
        //we can achieve abstraction by using interface and we can implement that interface in servie class

        // In solid principles we can use loose coupling in the application and we can achieve loose coupling by
        // using interface and dependency injection

        // what is loose coupling?
        // Loose coupling means the components of the application are independent of each other and they can be
        // changed without affecting the other components
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        //GET: GetAll
        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var products = await _productService.GetAll();
            return Ok(products);
        }
        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _productService.GetbyId(id);
            if(product == null)
            {
                return NotFound();//404
            }
            return Ok(product);//200
        }
        [HttpPost("Add")]
        public async Task<IActionResult> Add(ProductRequestAddDto productRequestAddDto)
        {
            var product = await _productService.Add(productRequestAddDto);
            if(product == null)
            {
                return BadRequest();//400
            }
            return Ok(product);
            //return CreatedAtAction(nameof(GetById),new { id = product.Id}, product);//201
        }
        [HttpPut("Update")]
        public async Task<IActionResult> Update(ProductRequestUpdateDto productRequestUpdateDto)
        {
            var product = await _productService.Update(productRequestUpdateDto);
            if(product == null)
            {
                return NotFound();//404
            }
            return Ok(product);//200
        }
        //Command and query architecture (CQRS) means we can separate the read and write operatoins in the application
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var isDeleted = await _productService.Delete(id);
            if(!isDeleted)
            {
                return NotFound();// 404 not found the resource is no found on the server
            }
            return NoContent();//204 no content means the request is successful but there is no content to return
        }
    }
    //what are http method?
    // getmethod is used to get the data from the database[httpGet]
    //post method is used to add the data to the database[httpPost]
    //put method is used to update the data in the database[httpPut]
    // delete method is used to delete the data from the database[httpDelete]

    // what are status code?
    // 200 ok means the request is successful and the response is returned successfully
    // 201 created means the request is successful and a new resource is created successfully
    // 400 bad request means the request is invalid and the server can't process the request
    // 404 not found means the requested resource is not found on the server
    // 500 internal server error means there is an error on the server and the request can't be processed
    // 204 deleted the record
}