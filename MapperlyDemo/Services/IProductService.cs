using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MapperlyDemo.DTOs;

namespace MapperlyDemo.Services
{
    public interface IProductService
    {
        Task<List<ProductResponseDTO>> GetAllAsync();
        Task<ProductResponseDTO> GetByIdAsync(int id);
        Task<ProductResponseDTO> CreateAsync(CreateProductRequestDTO dto);
        Task<ProductResponseDTO> UpdateAsync(UpdateProductRequestDTO dto);
    }
}