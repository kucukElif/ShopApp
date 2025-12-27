using System;
using System.Collections.Generic;
using System.Text;
using ShopApp.Bussiness.Abstract;
using ShopApp.DataAccess.Abstract;
using ShopApp.DataAccess.Concrete.EfCore;
using ShopApp.Entities;

namespace ShopApp.Bussiness.Concrete
{
    public class ProductManager : IProductService
    {
        private IProductDal _productDal;

       

        public ProductManager(IProductDal productDal)
        {
            _productDal = productDal;
        }
        public Product GetById(int id)
        {
            return _productDal.GetById(id);
        }

        public List<Product> GetAll()
        {
            return _productDal.GelAll().ToList();
        }

        public bool Create(Product product)
        {
            if (Validate(product)) {
                _productDal.Create(product);
                return true;
            }
            return false;

        }

        public void Update(Product product)
        {
            _productDal.Update(product);
        }

        public void Delete(Product product)
        {
           _productDal.Delete(product);
        }

        public Product GetProductDetails(int id)
        {
           return _productDal.GetProductDetails(id);
        }

        public List<Product> GetProductsByCategory(string category, int page, int pageSize)
        {
            return _productDal.GetProductsByCategory(category,page, pageSize);
        }

        public int GetCountByCategory(string category)
        {
            return _productDal.GetCountByCategory(category);
        }

        public Product GetByIdWithCategories(int id)
        {
            return _productDal.GetProductsByCategory(id);
        }

        public void Update(Product entity, int[] categoryIds)
        {
             _productDal.Update(entity, categoryIds);
        }
        public string ErrorMessage { get; set; }
        public bool Validate(Product entity)
        {
            var isValid = true;
            if (string.IsNullOrEmpty(entity.Name))
            {
                ErrorMessage += "ürün ismi girmelisiniz";
                isValid = false;
            }
            return isValid;
        }
    }
}
