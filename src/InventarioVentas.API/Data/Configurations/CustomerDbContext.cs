using InventarioVentas.API.Modules.Customers.Models;
using Microsoft.EntityFrameworkCore;


namespace InventarioVentas.API.Data.Configurations
{
    public class CustomerDbContext : DbContext
    {
        public CustomerDbContext(
            DbContextOptions<CustomerDbContext> options) : base(options)
        {
        }
        public DbSet<CustomerModel> Customers => Set<CustomerModel>();
    }
}
