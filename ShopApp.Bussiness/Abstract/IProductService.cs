using System;
using System.Collections.Generic;
using System.Text;
using ShopApp.Entities;

namespace ShopApp.Bussiness.Abstract
{
    public interface IProductService
    {
        Product GetById(int id);
        List<Product> GetAll();
        List<Product> GetProductsByCategory(string category, int page, int pageSize);
        void Create(Product product);
        void Update(Product product);
        void Delete(Product product);
        Product GetProductDetails(int id);
        int GetCountByCategory(string category);
       
    }
}
