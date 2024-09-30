namespace ApiStore.Models.Product
{
    public class ProductCreateViewModel
    {
        public string? Name { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        public List<IFormFile>? Images { get; set; }
    }
}
