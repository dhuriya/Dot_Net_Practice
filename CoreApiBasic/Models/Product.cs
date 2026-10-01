using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreApiBasic.Models
{
    public class Product
    {
        public int Id {get;set;}
        public string? Name{get; set;}
        public string? Category{get;set;}
        public double Price{get; set;}
        public bool InStock{get; set;}
    }
}