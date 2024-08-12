using Microsoft.AspNetCore.Mvc;
using WebHalk.Data;
using WebHalk.Data.Entities;
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
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CategoryCreateViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            string ext = System.IO.Path.GetExtension(model.Image.FileName);

            string fileName = Guid.NewGuid().ToString()+ext;

            var path = Path.Combine(Directory.GetCurrentDirectory(), "images", fileName);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await model.Image.CopyToAsync(stream);
            }

            CategoryEntity entity = new CategoryEntity()
            {
                Image = fileName,
                Name = model.Name,
            };
            _hulkDbContext.Categories.Add(entity);
            _hulkDbContext.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var item = _hulkDbContext.Categories.Find(id);

            if (item == null) return NotFound();

            _hulkDbContext.Categories.Remove(item);
            _hulkDbContext.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}
