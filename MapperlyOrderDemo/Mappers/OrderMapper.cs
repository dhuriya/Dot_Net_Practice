using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MapperlyOrderDemo.DTOs;
using MapperlyOrderDemo.Entities;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Riok.Mapperly.Abstractions;

namespace MapperlyOrderDemo.Mappers
{// The [Mapper] attribute instructs Mapperly to generate
    // mapping implementations for all partial methods in this class.
    [Mapper]
    public partial class OrderMapper
    {
        // ENTITY -> DTO MAPPING
        // This method maps:
        //   Order (Entity) -> OrderResponseDTO (API response)
        // 1. Complex -> Primitive flattening
        //    Order.Customer.FullName -> OrderResponseDTO.CustomerName
        [MapProperty(nameof(Order.Customer.FullName),
                     nameof(OrderResponseDTO.CustomerName))]
        // 2. Flatten ShippingAddress.City
        //    Order.ShippingAddress.City -> OrderResponseDTO.ShippingCity
        [MapProperty(nameof(Order.ShippingAddress.City),
                     nameof(OrderResponseDTO.ShippingCity))]
        // 3. Flatten ShippingAddress.State
        //    Order.ShippingAddress.State -> OrderResponseDTO.ShippingState
        [MapProperty(nameof(Order.ShippingAddress.State),
                     nameof(OrderResponseDTO.ShippingState))]
        // 4. Enum -> string conversion
        //    Order.Status (enum) -> OrderResponseDTO.Status (string)
        //    Uses helper method StatusToString
        [MapProperty(nameof(Order.Status),
                     nameof(OrderResponseDTO.Status),
                     Use = nameof(StatusToString))]
        // These source fields are intentionally not part of the response DTO.
        [MapperIgnoreSource(nameof(Order.CustomerId))]
        [MapperIgnoreSource(nameof(Order.IsPain))]
        [MapperIgnoreSource(nameof(Order.UpdatedOn))]
        [MapperIgnoreSource(nameof(Order.CreatedBy))]
        // Mapperly generates the method body automatically
        // using the mapping rules defined above.
        public partial OrderResponseDTO ToDto(Order order);
        // DTO -> ENTITY MAPPING (CREATE ORDER)
        // This method maps:
        //   CreateOrderRequestDTO -> Order (Entity)
        //
        // Design principles:
        // - Only ONE creation mapping (industry standard)
        // - Ignore system and computed fields
        // - Construct complex types explicitly
        // Ignore database-generated identity
        [MapperIgnoreTarget(nameof(Order.Id))]
        // Customer navigation is assigned in service layer
        [MapperIgnoreTarget(nameof(Order.Customer))]
        // Items collection is built in service after validation
        [MapperIgnoreTarget(nameof(Order.Items))]
        // Computed fields (business logic belongs to service)
        [MapperIgnoreTarget(nameof(Order.TotalAmount))]
        [MapperIgnoreTarget(nameof(Order.Status))]
        [MapperIgnoreTarget(nameof(Order.IsPain))]
        // Audit/system fields
        [MapperIgnoreTarget(nameof(Order.CreatedOn))]
        [MapperIgnoreTarget(nameof(Order.UpdatedOn))]
        [MapperIgnoreTarget(nameof(Order.CreatedBy))]
        // Primitive -> Complex mapping
        // CreateOrderRequestDTO has primitive shipping fields,
        // but Order expects a complex ShippingAddress object.
        [MapPropertyFromSource(nameof(Order.ShippingAddress),
                               Use = nameof(ToAddress))]
        // Mapperly generates the mapping implementation
        public partial Order ToEntity(CreateOrderRequestDTO dto);
        // DTO -> ENTITY (ORDER ITEM)
        // This method maps:
        //   CreateOrderItemRequestDTO -> OrderItem
        //
        // Only primitive fields are mapped here.
        // Navigation properties are handled in the service layer.
        [MapperIgnoreTarget(nameof(OrderItem.Id))]
        [MapperIgnoreTarget(nameof(OrderItem.OrderId))]
        // Order navigation set in service
        [MapperIgnoreTarget(nameof(OrderItem.Order))]
        // Product navigation loaded and assigned in service
        [MapperIgnoreTarget(nameof(OrderItem.Product))]
        // Unit price is loaded from the product in the service layer.
        [MapperIgnoreTarget(nameof(OrderItem.UnitPrice))]
        public partial OrderItem ToEntity(CreateOrderItemRequestDTO dto);
        // ENTITY -> DTO (ORDER ITEM RESPONSE)
        // This method maps:
        //   OrderItem -> OrderItemResponseDTO
        //
        // Purpose:
        // - Flatten Product navigation
        // - Compute line total
        // Product.Id -> ProductId
        [MapProperty(nameof(OrderItem.Product.Id),
                     nameof(OrderItemResponseDTO.ProductId))]
        // Product.Name -> ProductName
        [MapProperty(nameof(OrderItem.Product.Name),
                     nameof(OrderItemResponseDTO.ProductName))]
        //// Product.Price -> UnitPrice
        //[MapProperty(nameof(OrderItem.UnitPrice),
        //             nameof(OrderItemResponseDTO.UnitPrice))]
        // Computed field:
        // LineTotal = UnitPrice * Quantity
        [MapPropertyFromSource(nameof(OrderItemResponseDTO.LineTotal),
                               Use = nameof(CalcLineTotal))]
        // Mapperly generates this method
        public partial OrderItemResponseDTO ToDto(OrderItem item);
        // HELPER METHODS
        // These helpers are called by Mapperly during mapping.
        // They are NOT called manually.
        // Converts enum OrderStatus to string for API response
        private static string StatusToString(OrderStatus status)
        {
             return status.ToString();
        }
        // Builds the ShippingAddress complex object
        // from primitive DTO fields
        private static Address ToAddress(CreateOrderRequestDTO dto)
        {
            return new Address()
            {
                Line1 = dto.ShippingLine1,
                City = dto.ShippingCity,
                State = dto.ShippingState,
                Pincode = dto.ShippingPicode
            };
        }
        // Calculates LineTotal for an order item
        // Uses Product.Price and Quantity
        private static decimal CalcLineTotal(OrderItem item)
        {
            return item.UnitPrice * item.Quantity;
        } 
    }
}