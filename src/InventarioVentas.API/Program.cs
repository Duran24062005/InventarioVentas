using InventarioVentas.API.Data;
using InventarioVentas.API.Data.Configurations;
using InventarioVentas.API.Modules.Categories.Interfaces;
using InventarioVentas.API.Modules.Categories.Services;
using InventarioVentas.API.Modules.Customers.Interfaces;
using InventarioVentas.API.Modules.Customers.Services;
using InventarioVentas.API.Modules.Products.Interfaces;
using InventarioVentas.API.Modules.Products.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;


var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new InvalidOperationException(
        "La configuración 'ConnectionStrings:DefaultConnection' es obligatoria para usar PostgreSQL.");
}



// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddDbContext<CategoryDbContext>(options =>
    options.UseNpgsql(connectionString));

// Add services DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));
        

builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IProductService, ProductService>();  

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();
app.MapControllers();

app.Run();
