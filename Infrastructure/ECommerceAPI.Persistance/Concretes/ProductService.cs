using ECommerceAPI.Application.Abstractions;
using ECommerceAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceAPI.Persistance.Concretes
{
    //public class ProductService : IProductService    
    public class ProductService 
    {
        public List<Product> GetProducts()
            => new() {
                new (){ID=Guid.NewGuid(),Name="Product1",Price=100,Stock=10},
                new (){ID=Guid.NewGuid(),Name="Product2",Price=200,Stock=20},
                new (){ID=Guid.NewGuid(),Name="Product3",Price=300,Stock=30},
                new (){ID=Guid.NewGuid(),Name="Product4",Price=400,Stock=40},
                new (){ID=Guid.NewGuid(),Name="Product5",Price=500,Stock=50},
            };
    }
}
