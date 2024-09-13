using ApiStore.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace ApiStore.Data
{
    public class ApiStoreDbContext : DbContext
    {
        public ApiStoreDbContext(DbContextOptions<ApiStoreDbContext> options) 
            :base(options) { }

        public DbSet<CategoryEntity> Categories { get; set; }
    }
}
