namespace InventarioVentas.API.Data;

using InventarioVentas.API.Modules.Customers.Models;
using InventarioVentas.API.Modules.Categories.Models;
using InventarioVentas.API.Modules.Products.Models;
using InventarioVentas.API.Modules.Sales.Models;
using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<CustomerModel> Customers => Set<CustomerModel>();
    public DbSet<Sale> Sales => Set<Sale>();
    public DbSet<SaleDetails> SaleDetails => Set<SaleDetails>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }


}
