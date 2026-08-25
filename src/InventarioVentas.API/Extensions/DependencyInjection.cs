using InventarioVentas.API.Data;
using InventarioVentas.API.Data.Configurations;
using InventarioVentas.API.Modules.Categorias.Interfaces;
using InventarioVentas.API.Modules.Categorias.Services;
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
                "La configuración 'ConnectionStrings:DefaultConnection' es obligatoria.");
        }

        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(connectionString));

        services.AddDbContext<CategoriaDbContext>(options =>
            options.UseNpgsql(connectionString));

        services.AddScoped<ICategoriasService, CategoriasService>();

        return services;
    }
}
