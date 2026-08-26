using InventarioVentas.API.Data;
using InventarioVentas.API.Modules.Products.DTOs;
using InventarioVentas.API.Modules.Products.Interfaces;
using InventarioVentas.API.Modules.Products.Models;
using Microsoft.EntityFrameworkCore;

namespace InventarioVentas.API.Modules.Products.Services;

public class ProductService : IProductService
{
    private readonly AppDbContext _context;

    public ProductService(AppDbContext context)
    {
        _context = context;
    }

    // ============================================================
    // GET ALL
    // ============================================================

    public async Task<IEnumerable<ProductResponseDto>> GetAll()
    {
        return await _context.Products
            .Select(p => new ProductResponseDto
            {
                Id = p.Id,
                Name = p.Name,
                Code = p.Code,
                Price = p.Price,
                Stock = p.Stock,
                IsActive = p.IsActive,
                CategoryId = p.CategoryId,
                CreatedAt = p.CreatedAt
            })
            .ToListAsync();
    }

    // ============================================================
    // GET BY ID
    // ============================================================

    public async Task<ProductResponseDto?> GetById(Guid id)
    {
        var product = await _context.Products
            .FirstOrDefaultAsync(p => p.Id == id);

        if (product is null)
            return null;

        return new ProductResponseDto
        {
            Id = product.Id,
            Name = product.Name,
            Code = product.Code,
            Price = product.Price,
            Stock = product.Stock,
            IsActive = product.IsActive,
            CategoryId = product.CategoryId,
            CreatedAt = product.CreatedAt
        };
    }

    // ============================================================
    // CREATE
    // ============================================================

    public async Task<ProductResponseDto> Create(CreateProductDto dto)
    {
        var categoryExists = await _context.Categories
            .AnyAsync(c => c.Id == dto.CategoryId);

        if (!categoryExists)
            throw new InvalidOperationException("La categoría no existe");

        var product = new Product
        {
            Id = Guid.NewGuid(),
            Name = dto.Name,
            Code = dto.Code,
            Price = dto.Price,
            Stock = dto.Stock,
            IsActive = true,
            CategoryId = dto.CategoryId,
            CreatedAt = DateTime.UtcNow
        };

        _context.Products.Add(product);

        await _context.SaveChangesAsync();

        return new ProductResponseDto
        {
            Id = product.Id,
            Name = product.Name,
            Code = product.Code,
            Price = product.Price,
            Stock = product.Stock,
            IsActive = product.IsActive,
            CategoryId = product.CategoryId,
            CreatedAt = product.CreatedAt
        };
    }

    // ============================================================
    // UPDATE
    // ============================================================

    public async Task<bool> Update(Guid id, UpdateProductDto dto)
    {
        var product = await _context.Products
            .FirstOrDefaultAsync(p => p.Id == id);

        if (product is null)
            return false;

        product.Name = dto.Name;
        product.Code = dto.Code;
        product.Price = dto.Price;
        product.Stock = dto.Stock;
        product.IsActive = dto.IsActive;
        product.CategoryId = dto.CategoryId;

        await _context.SaveChangesAsync();

        return true;
    }

    // ============================================================
    // DELETE
    // ============================================================

    public async Task<bool> Delete(Guid id)
    {
        var product = await _context.Products
            .FirstOrDefaultAsync(p => p.Id == id);

        if (product is null)
            return false;

        _context.Products.Remove(product);

        await _context.SaveChangesAsync();

        return true;
    }
}