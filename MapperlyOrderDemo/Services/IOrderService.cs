using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MapperlyOrderDemo.DTOs;

namespace MapperlyOrderDemo.Services
{
    public interface IOrderService
    {
        Task<OrderResponseDTO> CreateAsync(CreateOrderRequestDTO dto);
        Task<OrderResponseDTO?> GetByIdAsync(int OrderId);
        Task<List<OrderResponseDTO>> GetByCustomerAsync(int customerId);
    }
}