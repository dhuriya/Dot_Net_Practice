using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MapperlyDemo.DTOs;
using MapperlyDemo.Entities;
using Riok.Mapperly.Abstractions;

namespace MapperlyDemo.Mapper
{
    [Mapper]
    public partial class ProductMapper
    {
        //Entity -> DTO
        public partial ProductResponseDTO ProductToProductDto(Product product);
        public partial List<ProductResponseDTO> ProductsToProductDtos(List<Product> products);
        // Create DTO -> Entity
        public partial Product CreateProductRequestDTOToProduct(CreateProductRequestDTO dto);
        // Update DTO -> existing Enity
        public partial void UpdateProductFromDTO(UpdateProductRequestDTO dto, Product product);
    }
}