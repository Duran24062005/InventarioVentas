using FluentValidation;
using InventarioVentas.API.Data;
using InventarioVentas.API.Modules.Categories.Interfaces;
using InventarioVentas.API.Modules.Categories.Services;
using InventarioVentas.API.Modules.Customers.Interfaces;
using InventarioVentas.API.Modules.Customers.Services;
using InventarioVentas.API.Modules.Products.Interfaces;
using InventarioVentas.API.Modules.Products.Services;
using InventarioVentas.API.Modules.Sales.Interfaces;
using InventarioVentas.API.Modules.Sales.Services;
using InventarioVentas.API.Modules.Sales.Validators;
using Microsoft.EntityFrameworkCore;

namespace InventarioVentas.API.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new InvalidOperationException(
                "La configuración 'ConnectionStrings:DefaultConnection' es obligatoria para usar PostgreSQL.");
        }

        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(connectionString));

        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<ICustomerService, CustomerService>();
        services.AddScoped<ISaleService, SaleService>();

        services.AddValidation();
        services.AddValidatorsFromAssemblyContaining<CreateSaleValidator>();

        return services;
    }
}
