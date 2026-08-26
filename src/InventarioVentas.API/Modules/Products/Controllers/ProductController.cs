using InventarioVentas.API.Modules.Products.DTOs;
using InventarioVentas.API.Modules.Products.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace InventarioVentas.API.Modules.Products.Controllers;

[ApiController]
[Route("api/products")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(
        IProductService productService)
    {
        _productService = productService;
    }

    // GET: api/products
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductResponseDto>>> GetAllAsync()
    {
        var products = await _productService.GetAll();

        return Ok(products);
    }

    // GET: api/products/{id}
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ProductResponseDto>> GetByIdAsync(Guid id)
    {
        var product = await _productService.GetById(id);

        if (product is null)
            return NotFound();

        return Ok(product);
    }

    // POST: api/products
    [HttpPost]
    public async Task<ActionResult<ProductResponseDto>> CreateAsync(
        CreateProductDto dto)
    {
        var product = await _productService.Create(dto);

        return CreatedAtAction(
            nameof(GetByIdAsync),
            new { id = product.Id },
            product);
    }

    // PUT: api/products/{id}
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateAsync(
        Guid id,
        UpdateProductDto dto)
    {
        var result = await _productService.Update(id, dto);

        if (!result)
            return NotFound();

        return NoContent();
    }

    // DELETE: api/products/{id}
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
        var result = await _productService.Delete(id);

        if (!result)
            return NotFound();

        return NoContent();
    }
}