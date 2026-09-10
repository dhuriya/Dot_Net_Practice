using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CrudOperationWithRepo.Services.IService;

namespace CrudOperationWithRepo.Services
{
    // Service layer is a layer in the application that contains the business logic of the application. It acts as a bridge
    //  between the controller layer and the repository/data access layer, processing requests and applying
    // business rules before interacting with the data layer.
    // here we write all the business logic of the application

    // here we aslo apply Encapsulation
    // Encapsulation is one of the fundamental principles of object-oriented programming (OOP) that promotes the bundling 
    // of data and methods that operate on that data into a single unit (class), while restricting direct
    // access to the internal state of the object.
    //Encapsulation means writing all the code in single unit or class
    public class ProductServices : IProductService
    {
        private readonly ApplicationDbContext _context;
        public ProductServices(ApplicationDbContext context)
        {
            _context = context;
        }
        public void Create(Product product)
        {
            _context.Products.Add(product);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var prudct = _context.Products.Find(id);
            if(prudct != null)
            {
                _context.Products.Remove(prudct);
                _context.SaveChanges();
            }
        }

        public IEnumerable<Product> GetAllProucts()
        {
            return _context.Products.ToList();
        }

        public Product GetById(int id)
        {
            var prudct = _context.Products.FirstOrDefault(p=>p.Id == id);
            return prudct;
        }

        public void Update(Product product)
        {
            var existPructs = _context.Products.Find(product.Id);
            if(existPructs != null)
            {
                existPructs.Name = product.Name;
                existPructs.Price = product.Price;
                existPructs.Quantity = product.Quantity;
                existPructs.Description = product.Description;
                _context.SaveChanges();
            }
        }
    }
    //here we can implement 2nd principle of solid which is open closed principle(OCP) oit means a class should be 
    // open for extension but closed for modification.


    // what is the solid principle in c#?
    // solid principles are a set of five design principles that help developers create maintainable and scalable software.
    // with help of solid principles we can achieve loose coupling, easy to test, and easy to maintain code.
    // The five solid principles are:
    //1. Single Responsibility Principle (SRP):
    // A class should have only one reason to change, meaing it should have only one responsibility.
    //2. Open/Close Principle (OCP):
    // it means a class should be open for extension but close for modification.

    //1 Liskove Substitution Principle (LSP):
}