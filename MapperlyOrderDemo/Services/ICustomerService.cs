using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MapperlyOrderDemo.DTOs;

namespace MapperlyOrderDemo.Services
{
    public interface ICustomerService
    {
        Task<CustomerResponseDTO> CreateAsync(CreateCustomerRequestDTO dto);
        Task<List<CustomerResponseDTO>> GetAllActiveAsync();
        Task<CustomerResponseDTO> GetByIdAsync(int id);
    }
}