using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CrudOperationWithDto.DTO.Request;
using CrudOperationWithDto.DTO.Response;

namespace CrudOperationWithDto.Interface
{
    // in Interface we just declare the method and we can do implementation in service class
    public interface IProductService
    {
        // Get all Products from the database
        Task<IEnumerable<ProductReponseDto>> GetAll();
        //Get Product by Id from the database
        Task<ProductReponseDto> GetbyId(int id);
        //Add Product to the database
        Task<ProductReponseDto> Add(ProductRequestAddDto productRequestAddDto);
        //Update Product in the database
        Task<ProductReponseDto> Update(ProductRequestUpdateDto productRequestUpdateDto);
        //Delete Product from the database
        Task<bool> Delete(int id);
    }
}