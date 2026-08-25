using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InventarioVentas.API.Modules.Categorias;
using InventarioVentas.API.Modules.Categorias.Models;

namespace InventarioVentas.API.Data.Configurations
{
    public class CategoriaDbContext : DbContext

    {
        public CategoriaDbContext(
                DbContextOptions<CategoriaDbContext> options)
            : base(options)
        { 
        }


        public DbSet<Categoria> Categoria => Set<Categoria>();
    }
}
