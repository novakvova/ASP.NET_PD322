using Microsoft.AspNetCore.Mvc;
using WebHalk.Data;
using WebHalk.Models.Categories;

namespace WebHalk.Controllers
{
    public class MainController : Controller
    {
        private readonly HulkDbContext _hulkDbContext;

        public MainController(HulkDbContext hulkDbContext)
        {
            _hulkDbContext = hulkDbContext;
        }
        public IActionResult Index()
        {
            var list = _hulkDbContext.Categories
                .Select(x=> new CategoryItemViewModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    Image = x.Image
                })
                .ToList();
            return View(list);
        }
    }
}
