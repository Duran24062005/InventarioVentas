using InventarioVentas.API.Data;
using InventarioVentas.API.Modules.Categories.DTOs;
using InventarioVentas.API.Modules.Categories.Interfaces;
using InventarioVentas.API.Modules.Categories.Models;
using Microsoft.EntityFrameworkCore;

namespace InventarioVentas.API.Modules.Categories.Services;

public class CategoryService : ICategoryService
{
    private readonly AppDbContext _context;

    public CategoryService(AppDbContext context)
    {
        _context = context;
    }

    // ============================================================
    // GET ALL
    // ============================================================

    public async Task<IEnumerable<CategoryResponseDto>> GetAll()
    {
        return await _context.Categories
            .Select(c => new CategoryResponseDto
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                CreatedAt = c.CreatedAt,
                IsActive = c.IsActive
            })
            .ToListAsync();
    }


    // ============================================================
    // GET BY ID
    // ============================================================

    public async Task<CategoryResponseDto?> GetById(Guid id)
    {
        var category = await _context.Categories
            .FirstOrDefaultAsync(c => c.Id == id);

        if (category is null)
            return null;

        return new CategoryResponseDto
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description,
            CreatedAt = category.CreatedAt,
            IsActive = category.IsActive
        };
    }


    // ============================================================
    // CREATE
    // ============================================================

    public async Task<CategoryResponseDto> Create(
        CreateCategoryDto dto)
    {
        var category = new Category
        {
            Id = Guid.NewGuid(),
            Name = dto.Name,
            Description = dto.Description,
            CreatedAt = DateTime.UtcNow,
            IsActive = true
        };

        _context.Categories.Add(category);

        await _context.SaveChangesAsync();

        return new CategoryResponseDto
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description,
            CreatedAt = category.CreatedAt,
            IsActive = category.IsActive
        };
    }


    // ============================================================
    // UPDATE
    // ============================================================

    public async Task<bool> Update(
        Guid id,
        UpdateCategoryDto dto)
    {
        var category = await _context.Categories
            .FirstOrDefaultAsync(c => c.Id == id);

        if (category is null)
            return false;

        category.Name = dto.Name;
        category.Description = dto.Description;
        category.IsActive = dto.IsActive;

        await _context.SaveChangesAsync();

        return true;
    }


    // ============================================================
    // DELETE
    // ============================================================

    public async Task<bool> Delete(Guid id)
    {
        var category = await _context.Categories
            .FirstOrDefaultAsync(c => c.Id == id);

        if (category is null)
            return false;

        _context.Categories.Remove(category);

        await _context.SaveChangesAsync();

        return true;
    }
}
