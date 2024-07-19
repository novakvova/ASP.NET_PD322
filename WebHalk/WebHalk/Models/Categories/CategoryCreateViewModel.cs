using System.ComponentModel.DataAnnotations;

namespace WebHalk.Models.Categories
{
    public class CategoryCreateViewModel
    {
        [Display(Name="Назва")]
        [Required(ErrorMessage ="Вкажіть назву категорії")]
        public string Name { get; set; } = string.Empty;
        [Display(Name = "Фото url")]
        [Required(ErrorMessage = "Вкажіть адерсу фото")]
        public string Image { get; set; } = string.Empty;
    }
}
