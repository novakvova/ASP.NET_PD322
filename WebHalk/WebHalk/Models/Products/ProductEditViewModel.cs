using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace WebHalk.Models.Products
{
    public class ProductEditViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = String.Empty;
        [Display(Name = "Вкажіть ціну хх,хх")]
        [RegularExpression(@"^\d+(\,\d{1,2})?$", ErrorMessage = "Введите корректное ціну.")]
        public string? Price { get; set; }
        public SelectList? CategoryList { get; set; }
        [Display(Name = "Category")]
        [Required(ErrorMessage = "Choose a category")]
        public int CategoryId { get; set; }
        public List<ProductImageViewModel>? Images { get; set; }

        [Display(Name = "New images")]
        public List<IFormFile>? NewImages { get; set; }

        [Display(Name = "Видалити фото")]
        public List<int>? DeletedPhotoIds { get; set; }
    }
}
