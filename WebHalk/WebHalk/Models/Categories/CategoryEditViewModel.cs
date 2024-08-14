using System.ComponentModel.DataAnnotations;

namespace WebHalk.Models.Categories
{
    public class CategoryEditViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Image { get; set; }

        [Display(Name = "Оберіть фото на ПК")]
        public IFormFile? NewImage { get; set; }
    }
}
