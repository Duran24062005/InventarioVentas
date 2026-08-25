using InventarioVentas.API.Data.Configurations;
using InventarioVentas.API.Modules.Categorias.Interfaces;
using InventarioVentas.API.Modules.Categorias.Services;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new InvalidOperationException(
        "La configuración 'ConnectionStrings:DefaultConnection' es obligatoria para usar PostgreSQL.");
}

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddDbContext<CategoriaDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddScoped<ICategoriasService, CategoriasService>();
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
