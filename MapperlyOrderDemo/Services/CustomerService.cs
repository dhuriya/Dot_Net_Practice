using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MapperlyOrderDemo.Data;
using MapperlyOrderDemo.DTOs;
using MapperlyOrderDemo.Mappers;
using Microsoft.EntityFrameworkCore;

namespace MapperlyOrderDemo.Services
{
    public class CustomerService : ICustomerService
    {
        // DbContext for database access
        private readonly AppDbContext _dbContext;
        //Mapperly-generated mapper for Customer mappings
        private readonly CustomerMapper _mapper;
        //Logger for structed logging
        private readonly ILogger<CustomerService> _logger;
        public CustomerService(AppDbContext dbContext, CustomerMapper mapper, 
        ILogger<CustomerService> logger)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
        }
        // CREATE CUSTOMER
        // Input : CreateCustomerRequestDTO (from API)
        // Output : CustomerResponseDTO (to API) 
        // Flow:
        // 1. Log request
        // 2. Validate busines rules
        // 3. Map DTO -> Entity using Mapperly
        // 4. Set system/business fields
        // 5. Persist to database
        // 6. Map Entity -> Response DTO
        public async Task<CustomerResponseDTO> CreateAsync(CreateCustomerRequestDTO dto)
        {
            try
            {
                //Log Request
                _logger.LogInformation("Creating customer. Mobile = {Mobile}", dto.Mobile);
                // Business validation (service responsibility)
                // Ensure mobile number is unique.
                // This rule belongs tin the service, NOT in the mapper.
                var mobileExists = await _dbContext.Customers.AnyAsync(c => c.Mobile == dto.Mobile);
                if(mobileExists)
                    throw new InvalidOperationException("A customer with the same mobile already exists.");
                //DTO -> Entity mapping (mapperly responsibility)
                var entity = _mapper.ToEntity(dto);
                //System / audit fields (service responsibility)
                // These values depend on runtime context and business rules,
                // so they are NOT handled by the mapper
                entity.IsActive = true;
                entity.RegisteredOn = DateTime.UtcNow;
                entity.CreatedBy = "SYSTEM";
                entity.CreatedOn = DateTime.UtcNow;
                // Persist entity
                _dbContext.Customers.Add(entity);
                await _dbContext.SaveChangesAsync();
                _logger.LogInformation("Customer Created sucessfully. CustomerId = {CustomerId}", entity.Id);
                return _mapper.ToDto(entity);
            }
            catch(Exception ex)
            {
                // Log full exception details for diagnostics
                _logger.LogError(ex, "Customer create failed Mobile = {Mobile}", dto.Mobile);
                throw;
            }
        }
        // GET ALL ACTIVE CUSTOMERS
        // Returns only active customers, sorted by latest registration

        public async Task<List<CustomerResponseDTO>> GetAllActiveAsync()
        {
            
            try
            {
                _logger.LogInformation("Fetching active customers.");
                // Query databse
                // - Filter: IsActive customers only 
                // - Sort: Most recentrly registered first
                var customers = await _dbContext.Customers.Where(c => c.IsActive).
                OrderByDescending(c => c.RegisteredOn).ToListAsync();
                return _mapper.ToDtos(customers);
                // Map Entity list -> DTO list
                // Mapperly maps each entity individually. 
                // return customer
                // .Select(customer => _mapper.ToDto(customer))
                // .ToList()
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Fetching active customers failed.");
                throw;
            }
        }
        // GET CUSTOMER BY ID
        // Returns null if customer does not exist or is inactive

        public async Task<CustomerResponseDTO> GetByIdAsync(int id)
        {
            
            try
            {
                _logger.LogInformation("Fetching customer by id. CustomerId ={CustomerId}", id);
                // Query single active customer
                var customer = await _dbContext.Customers
                .FirstOrDefaultAsync(c =>c.Id == id && c.IsActive);
                // Map only if entity exists
                return customer == null ? null : _mapper.ToDto(customer);

            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Fetching customer failed. CustomerId ={CustomerId}", id);
                throw;
            }
        }
    }
}