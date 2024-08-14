using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using WebHalk.Data;
using WebHalk.Mapper;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<HulkDbContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddAutoMapper(typeof(AppMapProfile));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

string dirSave = Path.Combine(Directory.GetCurrentDirectory(), "images");
if (!Directory.Exists(dirSave))
{
    Directory.CreateDirectory(dirSave); 
}


app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(dirSave),
    RequestPath = "/images"
});

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Main}/{action=Index}/{id?}");

app.Run();
