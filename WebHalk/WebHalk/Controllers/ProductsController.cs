using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Xml.Linq;
using WebHalk.Data;
using WebHalk.Data.Entities;
using WebHalk.Models.Products;

namespace WebHalk.Controllers
{
    public class ProductsController : Controller
    {
        private readonly HulkDbContext _context;
        private readonly IMapper _mapper;

        public ProductsController(HulkDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            var list = _context.Products
                  .ProjectTo<ProductItemViewModel>(_mapper.ConfigurationProvider)
                  .ToList() ?? throw new Exception("Failed to get products");

            return View(list);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var categories = _context.Categories
                .Select(x => new { Value = x.Id, Text = x.Name })
                .ToList();

            ProductCreateViewModel viewModel = new()
            {
                CategoryList = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(categories, "Value", "Text")
            };

            return View(viewModel);
        }


        [HttpPost]
        public async Task<IActionResult> Create(ProductCreateViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var prod = new ProductEntity
            {
                Name = model.Name,
                Price = model.Price,
                CategoryId = model.CategoryId,
            };

            await _context.Products.AddAsync(prod);
            await _context.SaveChangesAsync();
            if (model.Photos != null)
            {
                int i = 0;

                foreach (var img in model.Photos)
                {
                    string ext = System.IO.Path.GetExtension(img.FileName);
                    string fileName = Guid.NewGuid().ToString() + ext;
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "images", fileName);
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await img.CopyToAsync(stream);
                    }
                    var imgEntity = new ProductImageEntity
                    {
                        Image = fileName,
                        Priotity = i++,
                        Product = prod,
                    };
                    _context.ProductImages.Add(imgEntity);
                    _context.SaveChanges();
                }
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var model = _context.Products
                .ProjectTo<ProductEditViewModel>(_mapper.ConfigurationProvider)
                .FirstOrDefault(x => x.Id == id)
                ?? throw new Exception("An error occurred while receiving the product");

            var categories = _context.Categories
                .Select(x => new { Value = x.Id, Text = x.Name })
                .ToList();

            model.CategoryList = new SelectList(categories, "Value", "Text");

            return View(model);
        }

        //[HttpPost]
        //public IActionResult Edit(ProductEditViewModel model)
        //{

        //    var categories = _context.Categories
        //        .Select(x => new { Value = x.Id, Text = x.Name })
        //        .ToList();

        //    model.CategoryList = new SelectList(categories, "Value", "Text");

        //    return View(model);
        //}

        [HttpPost]
        public async Task<IActionResult> Edit(ProductEditViewModel model)
        {
            if (!ModelState.IsValid)
            {

                var editProduct = _context.Products
                    .ProjectTo<ProductEditViewModel>(_mapper.ConfigurationProvider)
                    .FirstOrDefault(x => x.Id == model.Id)
                    ?? throw new Exception("An error occurred while receiving the product");

                var categories = _context.Categories
                    .Select(x => new { Value = x.Id, Text = x.Name })
                    .ToList();

                model.CategoryList = new SelectList(categories, "Value", "Text");
                model.Images = editProduct.Images;

                return View(model);
            }



            var product = await _context.Products
                .FirstOrDefaultAsync(p => p.Id == model.Id)
                ?? throw new Exception("No product was found");
            _mapper.Map(model, product);

            if (model.NewImages != null)
            {
                foreach (var img in model.NewImages)
                {
                    if (img.Length > 0)
                    {
                        string ext = Path.GetExtension(img.FileName);
                        string fName = Guid.NewGuid().ToString() + ext;
                        var path = Path.Combine(Directory.GetCurrentDirectory(), "images", fName);

                        using (var fs = new FileStream(path, FileMode.Create))
                            await img.CopyToAsync(fs);

                        var imgEntity = new ProductImageEntity
                        {
                            Image = fName,
                            Product = product
                        };
                        _context.ProductImages.Add(imgEntity);
                    }
                }
            }

            if (model.DeletedPhotoIds != null)
            {
                var photos = _context.ProductImages
                    .Where(pi => model.DeletedPhotoIds.Contains(pi.Id))
                    .ToList();

                _context.ProductImages.RemoveRange(photos);

                foreach (var photo in photos)
                {
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "images", photo.Image);
                    if (System.IO.File.Exists(path)) System.IO.File.Delete(path);
                }
            }
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

    }
}
