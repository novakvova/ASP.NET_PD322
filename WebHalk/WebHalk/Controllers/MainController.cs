using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using WebHalk.Data;
using WebHalk.Data.Entities;
using WebHalk.Models.Categories;

namespace WebHalk.Controllers
{
    public class MainController : Controller
    {
        private readonly HulkDbContext _hulkDbContext;
        private readonly IMapper _mapper;

        public MainController(HulkDbContext hulkDbContext, IMapper mapper)
        {
            _hulkDbContext = hulkDbContext;
            _mapper = mapper;
        }
        public IActionResult Index()
        {
            var list = _hulkDbContext.Categories
                .ProjectTo<CategoryItemViewModel>(_mapper.ConfigurationProvider)
                //.Select(x=> new CategoryItemViewModel
                //{
                //    Id = x.Id,
                //    Name = x.Name,
                //    Image = x.Image
                //})
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


        [HttpGet]
        public IActionResult Edit(int id)
        {
            var item = _hulkDbContext.Categories
                .Where(c => c.Id == id)
                .ProjectTo<CategoryEditViewModel>(_mapper.ConfigurationProvider)
                .FirstOrDefault() ?? throw new InvalidDataException($"Item with such id={id} doesn`t exist");

            return View(item);
        }

        [HttpPost]
        public IActionResult Edit(CategoryEditViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var item = _hulkDbContext.Categories.Find(model.Id) ?? throw new InvalidDataException("Category not found");

            item.Name = model.Name;
            var images = Path.Combine(Directory.GetCurrentDirectory(), "images");

            if (model.NewImage != null)
            {
                var currentImg = Path.Combine(images, item.Image);
                if (System.IO.File.Exists(currentImg))
                    System.IO.File.Delete(currentImg);

                var newImg = Guid.NewGuid().ToString() + Path.GetExtension(model.NewImage.FileName);
                var newPath = Path.Combine(images, newImg);
                using (var stream = new FileStream(newPath, FileMode.Create))
                    model.NewImage.CopyTo(stream);

                item.Image = newImg;
            }

            _hulkDbContext.SaveChanges();
            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        public IActionResult Delete(int id)
        {
            var item = _hulkDbContext.Categories.Find(id);

            if (item == null) return NotFound();

            var path = Path.Combine(Directory.GetCurrentDirectory(), "images", item.Image);
            if (System.IO.File.Exists(path)) System.IO.File.Delete(path);


            _hulkDbContext.Categories.Remove(item);
            _hulkDbContext.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}
