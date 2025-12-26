using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using ShopApp.Bussiness.Abstract;
using ShopApp.WebUI.Models;


namespace ShopApp.WebUI.ViewComponents
{
    public class CategoryListViewComponent : ViewComponent
    {
        private ICategoryService _categoryService;

        public CategoryListViewComponent(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public IViewComponentResult Invoke()
        {
            var selectedCategory = ViewContext.RouteData.Values["category"]?.ToString();

            return View(new CategoryListViewModel()
            {
                SelectedCategory = selectedCategory,
                Categories = _categoryService.GetAll()
            });
        }
    }
}
