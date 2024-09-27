using ApiStore.Data;
using ApiStore.Data.Entities;
using ApiStore.Interfaces;
using ApiStore.Mapper;
using ApiStore.Services;
using Bogus;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApiStoreDbContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAutoMapper(typeof(AppMapProfile));
builder.Services.AddScoped<IImageHulk, ImageHulk>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseCors(opt =>
    opt.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

string imagesDirPath = Path.Combine(Directory.GetCurrentDirectory(), builder.Configuration["ImagesDir"]);

if (!Directory.Exists(imagesDirPath))
{
    Directory.CreateDirectory(imagesDirPath);
}

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(imagesDirPath),
    RequestPath = "/images"
});

app.UseAuthorization();

app.MapControllers();


#pragma warning restore ASP0014 // Suggest using top level route registrations

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApiStoreDbContext>();
    var imageHulk = scope.ServiceProvider.GetRequiredService<IImageHulk>();
    //dbContext.Database.EnsureDeleted();
    dbContext.Database.Migrate();

    if(dbContext.Categories.Count()==0)
    {
        int number = 10;
        var list = new Faker("uk")
            .Commerce.Categories(number);
        foreach(var name in list)
        {
            string image = imageHulk.Save("https://picsum.photos/1200/800?category").Result;
            var cat = new CategoryEntity
            {
                Name = name,
                Description = new Faker("uk").Commerce.ProductDescription(),
                Image = image
            };
            dbContext.Categories.Add(cat);
            dbContext.SaveChanges();
        }
    }

    if (dbContext.Products.Count() == 0)
    {
        var categories = dbContext.Categories.ToList();

        var fakerProduct = new Faker<ProductEntity>("uk")
            .RuleFor(u => u.Name, (f, u) => f.Commerce.Product())
            .RuleFor(u => u.Price, (f, u) => decimal.Parse(f.Commerce.Price()))
            .RuleFor(u => u.Category, (f, u) => f.PickRandom(categories));

        string url = "https://picsum.photos/1200/800?product";

        var products = fakerProduct.GenerateLazy(100);
        Random r = new Random();

        foreach (var product in products)
        {
            dbContext.Add(product);
            dbContext.SaveChanges();
            int imageCount = r.Next(3, 5);
            for (int i = 0; i < imageCount; i++)
            {
                var imageName = imageHulk.Save(url).Result;
                var imageProduct = new ProductImageEntity
                {
                    Product = product,
                    Image = imageName,
                    Priotity = i
                };
                dbContext.Add(imageProduct);
                dbContext.SaveChanges();
            }

        }
    }
}

app.Run();
