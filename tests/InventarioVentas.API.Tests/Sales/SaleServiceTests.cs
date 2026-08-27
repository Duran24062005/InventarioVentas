using InventarioVentas.API.Common.Exceptions;
using InventarioVentas.API.Modules.Customers.Models;
using InventarioVentas.API.Modules.Products.Models;
using InventarioVentas.API.Modules.Sales.DTOs;
using InventarioVentas.API.Modules.Sales.Services;
using InventarioVentas.API.Modules.Categories.Models;
using InventarioVentas.API.Tests.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace InventarioVentas.API.Tests.Sales;

public class SaleServiceTests : SqliteTestBase
{
    [Fact]
    public async Task Creates_sale_using_database_price_and_decrements_stock()
    {
        var customer = new CustomerModel
        {
            Id = Guid.NewGuid(),
            NombreCompleto = "Cliente de prueba",
            Documento = 1001,
            Email = "cliente@example.com",
            Telefono = "3000000000",
            FechaRegistro = DateTime.UtcNow
        };
        var category = new Category
        {
            Id = Guid.NewGuid(),
            Name = "Categoría de prueba",
            Description = "Datos de prueba",
            CreatedAt = DateTime.UtcNow,
            IsActive = true
        };
        var product = new Product
        {
            Id = Guid.NewGuid(),
            Name = "Producto de prueba",
            Code = "TEST-001",
            Price = 10.50m,
            Stock = 5,
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            CategoryId = category.Id
        };

        Context.AddRange(customer, category, product);
        await Context.SaveChangesAsync();

        var service = new SaleService(Context);
        var result = await service.Create(new CreateSaleDto
        {
            CustomerId = customer.Id,
            Details =
            [
                new CreateSaleDetailDto
                {
                    ProductId = product.Id,
                    Quantity = 2
                }
            ]
        });

        var persistedProduct = await Context.Products.SingleAsync(p => p.Id == product.Id);

        Assert.Equal(21.00m, result.Total);
        Assert.Equal(10.50m, result.Details.Single().UnitPrice);
        Assert.Equal(3, persistedProduct.Stock);
        Assert.Equal(1, await Context.Sales.CountAsync());
    }

    [Fact]
    public async Task Rejects_insufficient_stock_without_persisting_a_sale()
    {
        var customer = new CustomerModel
        {
            Id = Guid.NewGuid(),
            NombreCompleto = "Cliente de prueba",
            Documento = 1002,
            Email = "cliente2@example.com",
            Telefono = "3000000001",
            FechaRegistro = DateTime.UtcNow
        };
        var category = new Category
        {
            Id = Guid.NewGuid(),
            Name = "Categoría de prueba",
            Description = "Datos de prueba",
            CreatedAt = DateTime.UtcNow,
            IsActive = true
        };
        var product = new Product
        {
            Id = Guid.NewGuid(),
            Name = "Producto de prueba",
            Code = "TEST-002",
            Price = 20m,
            Stock = 1,
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            CategoryId = category.Id
        };

        Context.AddRange(customer, category, product);
        await Context.SaveChangesAsync();

        var service = new SaleService(Context);

        await Assert.ThrowsAsync<BusinessException>(() => service.Create(new CreateSaleDto
        {
            CustomerId = customer.Id,
            Details =
            [
                new CreateSaleDetailDto
                {
                    ProductId = product.Id,
                    Quantity = 2
                }
            ]
        }));

        var persistedProduct = await Context.Products.SingleAsync(p => p.Id == product.Id);

        Assert.Equal(1, persistedProduct.Stock);
        Assert.Empty(await Context.Sales.ToListAsync());
    }
}
