using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using ShopApp.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopApp.WebUI.Models
{
    public class ProductModel
    {
        public int Id { get; set; }
        //[Required]
        //[StringLength(50, MinimumLength =10,ErrorMessage ="Ürün ismi minimum 10 ve maksimum 60 karakter olmalıdır.")]
        public string Name { get; set; }
        [Required]
        public string ImageUrl { get; set; }
        [Required(ErrorMessage ="Fiyat belirtiniz.")]
        [Range(1,1000000)]
        public decimal? Price { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 20, ErrorMessage = "Ürün ismi minimum 50 ve maksimum 100 karakter olmalıdır.")]
        public string Description { get; set; }

        [ValidateNever]
        public List<Category> SelectedCategories { get; set; }
    }
}
