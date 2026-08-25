namespace InventarioVentas.API.Data;

using Microsoft.EntityFrameworkCore;
using InventarioVentas.API.Modules.Categorias.Models;
using InventarioVentas.API.Modules.Productos.Models;



public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Categoria> Categorias => Set<Categoria>();
    public DbSet<Producto> Productos => Set<Producto>();


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Se registra automaticamente todas las configuraciones Fluent API
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppContext).Assembly);
    }


}