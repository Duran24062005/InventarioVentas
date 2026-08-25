using InventarioVentas.API.Modules.Categories.DTOs;
using InventarioVentas.API.Modules.Categories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace InventarioVentas.API.Modules.Categories.Controllers;

[ApiController]
[Route("api/categorias")]
public class CategoriesController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoriesController(
        ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    // GET: api/categorias
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CategoryResponseDto>>> GetAll()
    {
        var categories = await _categoryService.GetAll();

        return Ok(categories);
    }

    // GET: api/categorias/{id}
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<CategoryResponseDto>> GetById(Guid id)
    {
        var category = await _categoryService.GetById(id);

        if (category is null)
            return NotFound();

        return Ok(category);
    }

    // POST: api/categorias
    [HttpPost]
    public async Task<ActionResult<CategoryResponseDto>> Create(
        CreateCategoryDto dto)
    {
        var category = await _categoryService.Create(dto);

        return CreatedAtAction(
            nameof(GetById),
            new { id = category.Id },
            category);
    }

    // PUT: api/categorias/{id}
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        Guid id,
        UpdateCategoryDto dto)
    {
        var result = await _categoryService.Update(id, dto);

        if (!result)
            return NotFound();

        return NoContent();
    }

    // DELETE: api/categorias/{id}
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
        var result = await _categoryService.Delete(id);

        if (!result)
            return NotFound();

        return NoContent();
    }
}
