using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CrudOperationWithDto.DTO.Request;
using CrudOperationWithDto.DTO.Response;

namespace CrudOperationWithDto.Automapper
{
    public class ProductMappingProfile : Profile
    {
        public ProductMappingProfile()
        {
            //CreateMap<Source, Destination>();
            // Mapping between Product and ProductDto
            // source here product and destination is ProductDto

            // getall api we are getting data from database product table and we want to send data to client in form of
            // ProductResponseDto
            CreateMap<Product,ProductReponseDto>();

            //Mapping from ProductRequestAddDto to Product

            // we can get data from client in form of ProductRequestAddDto and we want to save that data in database
            // in form of Product table
            CreateMap<ProductRequestAddDto,Product>();

            // Mapping from ProductRequestUpdateDto to Product
            // we can get data from client in form of ProductRequestUpdateDto and we want to update data in form of 
            // Product table
            CreateMap<ProductRequestUpdateDto,Product>();
        }
    }
}
// Why we use Mapper:
// To Convert Model => DTO models easily or vice versa
// automapper helps to write code in clean and maintainable way 
// it will ensure that api or in service we can't expose our database models to directly outside world

// it help to writing a clean architecture code and maintainable code.