using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JwtTokenBaseAuthentication.DTO;
using JwtTokenBaseAuthentication.Models;
using JwtTokenBaseAuthentication.Services.IServices;
using Microsoft.EntityFrameworkCore;

namespace JwtTokenBaseAuthentication.Services
{
    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext _context;
        public ProductService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<ProductResponseDto> Create(ProductRequesetDto productRequesetDto)
        {
            var product = new Product
            {
              Name = productRequesetDto.Name,
              Description = productRequesetDto.Description,
              Price = productRequesetDto.Price  
            };
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return new ProductResponseDto
            {
                Name = product.Name,
                Description = product.Description
            };
        }

        public async Task<string> Delete(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if(product ==null)
            {
                return "Product not fund";
            }
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return "Product deleted successfully";
        }

        public async Task<List<ProductResponseDto>> GetAll()
        {
            return await _context.Products.Select(p=> new ProductResponseDto
            {
                Name = p.Name,
                Description = p.Description,
            }).ToListAsync();
        }

        public async Task<ProductResponseDto> GetById(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if(product == null)
            {
                return null;
            }
            return new ProductResponseDto
            {
                Name = product.Name,
                Description = product.Description
            };
        }

        public async Task<ProductResponseDto> Update(ProductUpdateDto productUpdateDto)
        {
            var product = await _context.Products.FindAsync(productUpdateDto.Id);
            if(product == null)
            {
                return null;
            }
            product.Name = productUpdateDto.Name;
            product.Description = productUpdateDto.Description;
            product.Price = productUpdateDto.Price;
            await _context.SaveChangesAsync();
            return new ProductResponseDto
            {
                Name = product.Name,
                Description = product.Description
            };
        }
    }
}