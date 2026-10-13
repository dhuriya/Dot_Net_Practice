using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MapperlyOrderDemo.DTOs;
using MapperlyOrderDemo.Entities;
using Riok.Mapperly.Abstractions;

namespace MapperlyOrderDemo.Mappers
{
    public partial class CustomerMapper
    {
        //ENTITY -> DTO MAPPING
        //This method maps:
        //Customer (Entity) -> CustomerResponseDTO(Response DTO)

        //1. Different property name mapping
        // Entity.Mobile -> DTO.PhoneNumber
        [MapProperty(nameof(Customer.Mobile), nameof(CustomerResponseDTO.PhoneNumber))]
        //2. Complex -> Prmitive flattening
        // Entity.Address.City -> DTO.City
        [MapProperty(nameof(Customer.Address.City), nameof(CustomerResponseDTO.City))]
        //3. Complex - Primitive flattening
        // Entity.Address.State -> DTO.Sate
        [MapProperty(nameof(Customer.Address.State), nameof(CustomerResponseDTO.State))]
        //4. Fixed value mapping using source-based mapping
        // this does not read from Customer; instead , it calls 
        // GetSourceSystem(customer) ad assigns the returned value
        // to CustomerResponseDTO.SourceSystem
        [MapPropertyFromSource(nameof(CustomerResponseDTO.SourceSystem), Use = nameof(GetSourceSystem))]
        // 5. Dynamic value mapping
        // Execute GetMappedAtUtc(customer) at mapping time
        // to populate the response with the current UTC timestamp
        [MapPropertyFromSource(nameof(CustomerResponseDTO.MappedAtUtc), Use = nameof(GetMappedAtUtc))]

        // Mapperly will generate the implementation for this method
        // at compile tiem using the rules defined above
        public partial CustomerResponseDTO ToDto(Customer customer);
        // DTO -> ENTITY MAPPING (CREATE CUSTOMER)
        // This method maps:
        // CreateCustomerResquestDTO -> Customer(Entity)
        //
        //Important design decisions:
        // - Only ONE creation mapping is allowed
        // - Business/system fields are ignored
        // - complex type are constructed explicitly

        // 1. Null substituion mapping
        // if dto.Email is null or empty, EmailOrEmpty() ensures
        // the database never receives a null/invalid email value
        [MapProperty(nameof(CreateCustomerRequestDTO.Email), nameof(Customer.Email), Use = nameof(EmailOrEmpty))]
        // 2. Primitive -> Complex mapping
        // CreatecustomerRequestDTO contains primitive address fields,
        // but customer expects a complex Address object
        //
        // Mapperly calls ToAddress(dto) to construct Address.
        [MapPropertyFromSource(nameof(Customer.Address), Use = nameof(ToAddress))]

        // 3. Ignore database-generated and system-managed fields
        // these fields should be seet in the Service layer,
        // Not inside the mapper.
        [MapperIgnoreTarget(nameof(Customer.Id))]
        [MapperIgnoreTarget(nameof(Customer.RegisteredOn))]
        [MapperIgnoreTarget(nameof(Customer.IsActive))]
        [MapperIgnoreTarget(nameof(Customer.CreatedBy))]
        [MapperIgnoreTarget(nameof(Customer.CreatedOn))]
        [MapperIgnoreTarget(nameof(Customer.UpdatedOn))]
        [MapperIgnoreTarget(nameof(Customer.Orders))]
        //Mapperly generates the implementation for this method.
        public partial Customer ToEntity(CreateCustomerRequestDTO dto);
        //HELPER METHODS USED BY MAPPERLY
        // These methods are NOT called manually.
        // Mapperly invokes then automatically during mapping

        //Builds the address complex type from primitive DTO fields
        //Used in Primitive -> Complex mapping
        private static Address ToAddress(CreateCustomerRequestDTO dto)
        {
            return new Address()
            {
                Line1 = dto.AddressLine1,
                City = dto.City,
                State = dto.State,
                Pincode = dto.Pincode
            };
        }
        // Ensure Email is never null or whitespace
        // Demostrates null substitution logic
        private static string? EmailOrEmpty(string? email) => string.IsNullOrWhiteSpace(email) ? "NA": email;

        //Returns a fixed source-system identifier
        // Useful in distributed systems / autiting
        private static string GetSourceSystem(Customer _) => "MapperlyOrderDemo";

        //Returns the current UTC timestamp at mapping time 
        // Demonstrates dynamic value assignment
        private static DateTime GetMappedAtUtc(Customer _) => DateTime.UtcNow;
    }
}