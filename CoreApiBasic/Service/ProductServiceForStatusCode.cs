using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreApiBasic.Models;

namespace CoreApiBasic.Service
{
    public static class ProductServiceForStatusCode
    {
        private static readonly List<Product> _products = new()
        {
            new Product { Id = 1, Name = "HP Laptop", Category = "Electronics", Price = 55000, InStock = true },
            new Product { Id = 2, Name = "iPhone 15", Category = "Mobiles", Price = 125000, InStock = true },
            new Product { Id = 3, Name = "Samsung TV", Category = "Electronics", Price = 78000, InStock = false },
            new Product { Id = 4, Name = "Nike Shoes", Category = "Footwear", Price = 8500, InStock = true },
            new Product { Id = 5, Name = "Levi’s Jeans", Category = "Clothing", Price = 4500, InStock = true }
        };
        public static async Task<int> GetProductCountAsync()
        {
            await Task.Delay(300);
            return _products.Count();
        }
        public static async Task<List<Product>> GetAllProductAsync()
        {
            await Task.Delay(400);
            return _products;
        }
        public static async Task<Product> GetProductByIdAsync(int id)
        {
            await Task.Delay(300);
            return _products.FirstOrDefault(p =>p.Id == id);
        }
        public static async Task<List<string>> GetProductAllNameAsync()
        {
            await Task.Delay(300);
            return _products.Select(p => p.Name).ToList();
        }
    }
}