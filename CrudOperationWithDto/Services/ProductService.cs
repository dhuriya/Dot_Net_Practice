using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CrudOperationWithDto.DTO.Request;
using CrudOperationWithDto.DTO.Response;
using CrudOperationWithDto.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace CrudOperationWithDto.Services
{
    // Encapsulation means wrapping the data and the method together as a single unit and restricting the access to the data
    // from outside the class
    public class ProductService: IProductService
    {
        // Privare access modifier means we can access the variable or method only within the class
        // and we can't access it from outside the class
        private  readonly ApplicationDbContext _context;
        public ProductService(ApplicationDbContext context)
        {
            _context = context;
        }
        //Public access modifier means we can access the variable or method from anywhere in the application
        public async Task<ProductReponseDto> Add(ProductRequestAddDto productRequestAddDto)
        {
            var product = new Product
            {
                Name = productRequestAddDto.Name,
                Price = productRequestAddDto.Price,
                Quantity = productRequestAddDto.Quantity,
                Description = productRequestAddDto.Description
            };
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
            return new ProductReponseDto
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Quantity = product.Quantity,
                Description = product.Description
            };
        }

        public async Task<bool> Delete(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if(product == null)
            {
                return false;
            }
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return true;
        }

        //Get All Product from the database
        public async Task<IEnumerable<ProductReponseDto>> GetAll()
        {
            
            return await _context.Products.Select(p => new ProductReponseDto
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                Quantity = p.Quantity,
                Description = p.Description
            }).ToListAsync();
        }

        public async Task<ProductReponseDto> GetbyId(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if(product == null)
            {
                return null;
            }
            return new ProductReponseDto
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Quantity = product.Quantity,
                Description = product.Description
            };
        }

        public async Task<ProductReponseDto> Update(ProductRequestUpdateDto productRequestUpdateDto)
        {
            var product = await _context.Products.FindAsync(productRequestUpdateDto.Id);
            if(product == null)
            {
                return null;
            }
            product.Name = productRequestUpdateDto.Name;
            product.Quantity = productRequestUpdateDto.Quantity;
            product.Price = productRequestUpdateDto.Price;
            product.Description = productRequestUpdateDto.Description;
            await _context.SaveChangesAsync();
            return new ProductReponseDto
            {
                Id = product.Id,
                Name = product.Name,
                Quantity = product.Quantity,
                Price = product.Price,
                Description = product.Description
            };
        }
    }
}