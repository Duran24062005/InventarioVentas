using Microsoft.AspNetCore.Mvc;
using InventarioVentas.API.Modules.Categorias.DTOs;
using InventarioVentas.API.Modules.Categorias.Interfaces;

namespace InventarioVentas.API.Modules.Categorias.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriasController : ControllerBase
{
    private readonly ICategoriasService _categoriasService;

    public CategoriasController(
        ICategoriasService categoriasService)
    {
        _categoriasService = categoriasService;
    }

    // GET: api/categorias
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoriaReposeDto>>> GetAllAsync()
    {
        var categorias = await _categoriasService.GetAllAsync();

        return Ok(categorias);
    }

    // GET: api/categorias/{id}
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<CategoriaReposeDto>> GetByIdAsync(Guid id)
    {
        var categoria = await _categoriasService.GetByIdAsync(id);

        if (categoria is null)
            return NotFound();

        return Ok(categoria);
    }

    // POST: api/categorias
    [HttpPost]
    public async Task<ActionResult<CategoriaReposeDto>> CreateAsync(
        CrearCategoriaDto dto)
    {
        var categoria = await _categoriasService.CreateAsync(dto);

        return CreatedAtAction(
            nameof(GetByIdAsync),
            new { id = categoria.Id },
            categoria);
    }

    // PUT: api/categorias/{id}
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateAsync(
        Guid id,
        ActualizarCategoriaDto dto)
    {
        var result = await _categoriasService.UpdateAsync(id, dto);

        if (!result)
            return NotFound();

        return NoContent();
    }

    // DELETE: api/categorias/{id}
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
        var result = await _categoriasService.DeleteAsync(id);

        if (!result)
            return NotFound();

        return NoContent();
    }
}