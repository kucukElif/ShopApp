using Microsoft.AspNetCore.Mvc;
using ShopApp.Bussiness.Abstract;
using ShopApp.Entities;
using ShopApp.WebUI.Models;


namespace ShopApp.WebUI.Controllers
{
    public class Shop : Controller
    {
        private IProductService _productService;
        public Shop(IProductService productService)
        {
            _productService = productService;
        }
        public IActionResult Details(int id)
        {
            var product = _productService.GetProductDetails(id);

            if (product == null)
                return NotFound();

            var model = new ProductDetailsModel
            {
                Product = product,
                Categories = product.ProductCategories
                                    .Select(i => i.Category)
                                    .ToList()
            };

            return View(model);

        }

        public IActionResult List(string category, int page = 1)
        {
            const int pageSize = 3;
            return View(new ProductListModel()
            {
                PageInfo = new PageInfo()
                {
                    TotalItems = _productService.GetCountByCategory(category),
                    CurrentPage = page,
                    ItemsPerPage = pageSize,
                    CurrentCategory = category
                },
                Products = _productService.GetProductsByCategory(category, page, pageSize)
            });

        }

    }
}
