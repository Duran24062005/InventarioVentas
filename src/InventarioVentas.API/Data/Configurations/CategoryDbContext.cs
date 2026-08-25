using InventarioVentas.API.Modules.Categories.Models;
using Microsoft.EntityFrameworkCore;

namespace InventarioVentas.API.Data.Configurations
{
    public class CategoryDbContext : DbContext

    {
        public CategoryDbContext(
                DbContextOptions<CategoryDbContext> options)
            : base(options)
        { 
        }


        public DbSet<Category> Categories => Set<Category>();
    }
}
