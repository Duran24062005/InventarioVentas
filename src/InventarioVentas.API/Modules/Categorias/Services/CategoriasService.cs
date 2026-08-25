using InventarioVentas.API.Data.Configurations;
using InventarioVentas.API.Modules.Categorias.DTOs;
using InventarioVentas.API.Modules.Categorias.Interfaces;
using InventarioVentas.API.Modules.Categorias.Models;
using Microsoft.EntityFrameworkCore;

namespace InventarioVentas.API.Modules.Categorias.Services;

public class CategoriasService : ICategoriasService
{
    private readonly CategoriaDbContext _context;

    public CategoriasService(CategoriaDbContext context)
    {
        _context = context;
    }

    // ============================================================
    // GET ALL
    // ============================================================

    public async Task<IEnumerable<CategoriaReposeDto>> GetAllAsync()
    {
        return await _context.Categoria
            .Select(c => new CategoriaReposeDto
            {
                Id = c.Id,
                Nombre = c.Nombre,
                Descripcion = c.Descripcion,
                FechaCreacion = c.FechaCreacion,
                Estado = c.Estado
            })
            .ToListAsync();
    }


    // ============================================================
    // GET BY ID
    // ============================================================

    public async Task<CategoriaReposeDto?> GetByIdAsync(Guid id)
    {
        var categoria = await _context.Categoria
            .FirstOrDefaultAsync(c => c.Id == id);

        if (categoria is null)
            return null;

        return new CategoriaReposeDto
        {
            Id = categoria.Id,
            Nombre = categoria.Nombre,
            Descripcion = categoria.Descripcion,
            FechaCreacion = categoria.FechaCreacion,
            Estado = categoria.Estado
        };
    }


    // ============================================================
    // CREATE
    // ============================================================

    public async Task<CategoriaReposeDto> CreateAsync(
        CrearCategoriaDto dto)
    {
        var categoria = new Categoria
        {
            Id = Guid.NewGuid(),
            Nombre = dto.Nombre,
            Descripcion = dto.Descripcion,
            FechaCreacion = DateTime.UtcNow,
            Estado = true
        };

        _context.Categoria.Add(categoria);

        await _context.SaveChangesAsync();

        return new CategoriaReposeDto
        {
            Id = categoria.Id,
            Nombre = categoria.Nombre,
            Descripcion = categoria.Descripcion,
            FechaCreacion = categoria.FechaCreacion,
            Estado = categoria.Estado
        };
    }


    // ============================================================
    // UPDATE
    // ============================================================

    public async Task<bool> UpdateAsync(
        Guid id,
        ActualizarCategoriaDto dto)
    {
        var categoria = await _context.Categoria
            .FirstOrDefaultAsync(c => c.Id == id);

        if (categoria is null)
            return false;

        categoria.Nombre = dto.Nombre;
        categoria.Descripcion = dto.Descripcion;
        categoria.Estado = dto.Estado;

        await _context.SaveChangesAsync();

        return true;
    }


    // ============================================================
    // DELETE
    // ============================================================

    public async Task<bool> DeleteAsync(Guid id)
    {
        var categoria = await _context.Categoria
            .FirstOrDefaultAsync(c => c.Id == id);

        if (categoria is null)
            return false;

        _context.Categoria.Remove(categoria);

        await _context.SaveChangesAsync();

        return true;
    }
}