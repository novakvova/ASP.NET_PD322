using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace WebHalk.Models.Products
{
    public class ProductCreateViewModel
    {
        [Display(Name = "Назва")]
        [Required(ErrorMessage = "Вкажіть назву продукту")]
        [StringLength(500, ErrorMessage = "Назва продукту не повинна перевищувати 500 символів")]
        public string Name { get; set; } = string.Empty;
        [Display(Name = "Ціна")]
        public decimal Price { get; set; }
        [Display(Name = "Категорія")]
        [Required(ErrorMessage = "Оберіть категорію")]
        public int CategoryId { get; set; }
        [Display(Name = "Фото")]
        public List<IFormFile>? Photos { get; set; }

        //список усіх категорій, які відобрається для продукта
        public SelectList? CategoryList { get; set; }
    }
}
