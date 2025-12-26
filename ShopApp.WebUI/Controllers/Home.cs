using Microsoft.AspNetCore.Mvc;
using ShopApp.Bussiness.Abstract;
using ShopApp.WebUI.Models;

namespace ShopApp.WebUI.Controllers
{
    public class Home : Controller
    {
        private IProductService _productService;
        private ICategoryService _categoryService;

        public Home(IProductService productService)
        {
            _productService = productService;
  
        }
        public IActionResult Index()
        {
            return View(new ProductListModel()
            {
                Products = _productService.GetAll(),
              
            });
        }
    }
}
