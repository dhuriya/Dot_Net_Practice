using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CrudOperationWithDto;
using Microsoft.EntityFrameworkCore;

namespace CrudOperationOnionArchitecture.DataAccessLayer.DataAccess
{
    // ApplicationDbContext class inherits from DbContext , which is a part of Entity Framework Core.
    // It is a class in of ef core that acts like a bridge between the application and the database.
    // it manages the database connection and provides methods for querying and saving data.
    public class ApplicationDbContext: DbContext
    {
      public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
        {
            
        }
        public DbSet<Product> Products{get;set;}
    }
}