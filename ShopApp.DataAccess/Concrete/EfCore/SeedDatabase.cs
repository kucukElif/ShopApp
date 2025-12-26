using Microsoft.EntityFrameworkCore;
using ShopApp.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopApp.DataAccess.Concrete.EfCore
{
    public static class SeedDatabase
    {
        public static void Seed()
        {
            var context = new ShopContext();
            if (context.Database.GetPendingMigrations().Count() == 0)
            {
                if (context.Categories.Count() == 0)
                {
                    context.Categories.AddRange(Categories);
                    
                }
                if (context.Products.Count() == 0)
                {
                    context.Products.AddRange(Products);
                    context.AddRange(ProductCategory);
                    
                }
                context.SaveChanges();
            }
            
        }
        public static Category[] Categories =
       {
            new Category() { CategoryName = "Telefon" },
            new Category() { CategoryName = "Bilgisayar" },
            new Category() { CategoryName = "Elektronik" },
        };
        public static Product[] Products =
     {
            new Product() { Name = "Samsung s5", Price=2000,ImageUrl="1.jpg", Description="Güzel Telefon" },
            new Product() { Name = "Samsung s6", Price=2000,ImageUrl="2.jpg", Description="Güzel Telefon" },
            new Product() { Name = "Samsung s7", Price=2000,ImageUrl="3.jpg", Description="Güzel Telefon" },
            new Product() { Name = "Samsung s8", Price=2000,ImageUrl="4.jpg", Description="Güzel Telefon" },
            new Product() { Name = "Samsung s9", Price=2000,ImageUrl="5.jpg", Description="Güzel Telefon" },
            new Product() { Name = "IPhone 6", Price=2000,ImageUrl="6.jpg", Description="Güzel Telefon"},
            new Product() { Name = "IPhone 7", Price=2000,ImageUrl="7.jpg", Description="Güzel Telefon"},

        };

        private static ProductCategory[] ProductCategory =
        {
            new ProductCategory(){Product=Products[0], Category = Categories[0]},
            new ProductCategory(){Product=Products[0], Category = Categories[2]},
            new ProductCategory(){Product=Products[1], Category = Categories[0]},
            new ProductCategory(){Product=Products[1], Category = Categories[1]},
            new ProductCategory(){Product=Products[2], Category = Categories[0]},
            new ProductCategory(){Product=Products[2], Category = Categories[2]},
            new ProductCategory(){Product=Products[2], Category = Categories[1]},
            new ProductCategory(){Product=Products[3], Category = Categories[1]},
        };
    }
}

   


