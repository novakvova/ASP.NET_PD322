using ApiStore.Interfaces;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Webp;
using SixLabors.ImageSharp.Processing;

namespace ApiStore.Services
{
    public class ImageHulk(IConfiguration configuration) : IImageHulk
    {
        public async Task<string> Save(IFormFile image)
        {
            string imageName = Guid.NewGuid().ToString() + ".webp";
            var dir = configuration["ImagesDir"];
            
            using (MemoryStream ms = new())
            {
                await image.CopyToAsync(ms);
                var bytes = ms.ToArray();
                var sizes = configuration["ImageSizes"].Split(",")
                    .Select(x => int.Parse(x));
                //int[] sizes = [50, 150, 300, 600, 1200];
                foreach (var size in sizes)
                {
                    string dirSave = Path.Combine(Directory.GetCurrentDirectory(),
                        dir, $"{size}_{imageName}");
                    using (var imageLoad = Image.Load(bytes))
                    {
                        // Resize the image (50% of original dimensions)
                        imageLoad.Mutate(x => x.Resize(new ResizeOptions
                        {
                            Size = new Size(size, size),
                            Mode = ResizeMode.Max
                        }));

                        // Save the image with compression
                        imageLoad.Save(dirSave, new WebpEncoder());
                    }
                }
            }
            
            return imageName;
        }
    }
}
