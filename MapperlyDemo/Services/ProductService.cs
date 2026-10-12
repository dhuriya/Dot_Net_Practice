using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MapperlyDemo.DTOs;
using MapperlyDemo.Entities;
using MapperlyDemo.Mapper;

namespace MapperlyDemo.Services
{
    public class ProductService : IProductService
    {
        private readonly ProductMapper _mapper;
        // Static in-memory store (shared across requests)
        private static readonly List<Product> _products = new();

        public async Task<List<ProductResponseDTO>> GetAllAsync()
        {
            await Task.Delay(TimeSpan.FromSeconds(1));
            var result = _mapper.ProductsToProductDtos(_products);
            return result;
        }
        public async Task<ProductResponseDTO?> GetByIdAsync(int id)
        {
            await Task.Delay(TimeSpan.FromSeconds(1));
            var product = _products.FirstOrDefault(p => p.Id == id);
            return product is null ? null : _mapper.ProductToProductDto(product);
        }
        public async Task<ProductResponseDTO> CreateAsync(CreateProductRequestDTO dto)
        {
            await Task.Delay(TimeSpan.FromSeconds(1));
            var product = _mapper.CreateProductRequestDTOToProduct(dto);
            product.Id = _products.Count + 1;
            product.CreatedOn = DateTime.Now;
            _products.Add(product);
            return _mapper.ProductToProductDto(product);
        }

        public async Task<ProductResponseDTO> UpdateAsync(UpdateProductRequestDTO dto)
        {
            await Task.Delay(TimeSpan.FromSeconds(1));
            var product = _products.FirstOrDefault(p => p.Id == dto.Id);
            if(product is null)
            {
                return null;
            }
            _mapper.UpdateProductFromDTO(dto, product);
            return _mapper.ProductToProductDto(product);
        }
    }
} 