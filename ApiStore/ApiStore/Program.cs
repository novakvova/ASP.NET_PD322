using ApiStore.Data;
using ApiStore.Data.Entities;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApiStoreDbContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();


#pragma warning restore ASP0014 // Suggest using top level route registrations

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApiStoreDbContext>();
    //dbContext.Database.EnsureDeleted();
    dbContext.Database.Migrate();

    if(dbContext.Categories.Count()==0)
    {
        var cat = new CategoryEntity
        {
            Name="Собаки",
            Description="Собаки на довільні впобання та розмір.",
            Image="dog.jpg"
        };
        dbContext.Categories.Add(cat);
        dbContext.SaveChanges();
    }
}

app.Run();
