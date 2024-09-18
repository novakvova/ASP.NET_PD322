using ApiStore.Data;
using ApiStore.Data.Entities;
using ApiStore.Models.Category;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Formats.Webp;
using SixLabors.ImageSharp.Processing;

namespace ApiStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController(
        ApiStoreDbContext context, IConfiguration configuration,
        IMapper mapper) : ControllerBase
    {
        [HttpGet]
        public IActionResult GetList()
        {
            var list = context.Categories.ToList();
            return Ok(list);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm]CategoryCreateViewModel model)
        {
            string imageName = Guid.NewGuid().ToString()+".webp";
            var dir = configuration["ImagesDir"];
            
            using (MemoryStream ms = new())
            {
                await model.Image.CopyToAsync(ms);
                var bytes = ms.ToArray();
                int[] sizes = [50, 150, 300, 600, 1200];
                foreach (var size in sizes)
                {
                    string dirSave = Path.Combine(Directory.GetCurrentDirectory(),
                        dir, $"{size}_{imageName}");
                    using (var image = Image.Load(bytes))
                    {
                        // Resize the image (50% of original dimensions)
                        image.Mutate(x => x.Resize(new ResizeOptions
                        {
                            Size = new Size(size, size),
                            Mode = ResizeMode.Max
                        }));

                        // Save the image with compression
                        image.Save(dirSave,  new WebpEncoder());
                    }
                }
            }
            var entity =  mapper.Map<CategoryEntity>(model);
            entity.Image = imageName;
            context.Categories.Add(entity);
            context.SaveChanges();
            return Ok();
        }
    }
}
