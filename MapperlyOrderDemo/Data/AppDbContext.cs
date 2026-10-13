using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MapperlyOrderDemo.Entities;
using Microsoft.EntityFrameworkCore;

namespace MapperlyOrderDemo.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options){} 
        public DbSet<Customer> Customers {get; set;}
        public DbSet<Product> Products {get; set;}
        public DbSet<Order> Orders {get; set;}
        public DbSet<OrderItem> OrderItems {get; set;}
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>().OwnsOne(c => c.Address);
            modelBuilder.Entity<Order>().OwnsOne(o => o.ShippingAddress);

            var seedUtc = new DateTime(2026, 01, 01, 0, 0, 0, DateTimeKind.Utc);
            modelBuilder.Entity<Customer>().HasData(
                new Customer { Id = 1, FullName ="Ravi Kumar", Mobile="9876543216", Email = "Ravi@gmail.com"},
                new Customer { Id = 2, FullName ="Sita Sharma", Mobile="9123456789", Email = "Sita@gmail.com"}
            );
            modelBuilder.Entity<Customer>().OwnsOne(c => c.Address).HasData(
                new { CustomerId = 1, Line1 = "MG Road", City = "Bengaluru", State = "Karnataka" },
                new { CustomerId = 2, Line1 = "Park Street", City = "Kolkata", State = "West Bangal" }
            );
            //Seed Products
            modelBuilder.Entity<Product>().HasData(
                new Product {Id = 1, Name ="Wireless Mouse", Price = 599},
                new Product{ Id = 2, Name = "Keyboard", Price = 899},
                new Product { Id = 3, Name = "USB Cable", Price = 199}
            );
            base.OnModelCreating(modelBuilder);
        }
    }
}