using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CrudOperationWithRepo.Services.IService
{
    // what is the interface in C#?
    // an interface we can declare the method but we can't provide the implementation of the method in the interface.
    // the implementation will be provided by the class that implements the interface.\

    // What is the abtraction in C#?
    // we can achieve the abstraction with the help of interface it means we can hide the implementation details
    // and only show the functionality to the user.

    // how we can achieve the dependency injection in C#?
    // so we can acieve the di with the interface

    // how we can achieve the multiple inheritance in C#?
    // in C# we can achieve multiple inheritance with the help of interfaces.
    public interface IProductService
    {
        //Get all the Products
        IEnumerable<Product> GetAllProucts();
        // Get a product by id
        Product GetById(int id);
        // Create a new product
        void Create(Product product);
        void Update(Product product);
        void Delete(int id);
    }
}