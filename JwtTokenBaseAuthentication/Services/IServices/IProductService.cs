using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JwtTokenBaseAuthentication.DTO;

namespace JwtTokenBaseAuthentication.Services.IServices
{
    public interface IProductService
    {
        Task<ProductResponseDto> Create(ProductRequesetDto productRequesetDto);
        Task<List<ProductResponseDto>> GetAll();
        Task<ProductResponseDto> Update(ProductUpdateDto productUpdateDto);
        Task<string> Delete(int id);
        Task<ProductResponseDto> GetById(int id);
    }
}