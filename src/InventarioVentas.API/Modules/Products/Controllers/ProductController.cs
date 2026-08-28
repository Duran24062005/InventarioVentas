using InventarioVentas.API.Modules.Products.DTOs;
using InventarioVentas.API.Modules.Products.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace InventarioVentas.API.Modules.Products.Controllers;

[ApiController]
[Route("api/productos")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(
        IProductService productService)
    {
        _productService = productService;
    }

    // GET: api/productos
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductResponseDto>>> GetAll()
    {
        var products = await _productService.GetAll();

        return Ok(products);
    }

    // GET: api/productos/{id}
    [HttpGet("{id:guid}", Name = "GetProductById")]
    public async Task<ActionResult<ProductResponseDto>> GetById(Guid id)
    {
        var product = await _productService.GetById(id);

        if (product is null)
            return NotFound();

        return Ok(product);
    }

    // POST: api/productos
    [HttpPost]
    public async Task<ActionResult<ProductResponseDto>> Create(
        CreateProductDto dto)
    {
        var product = await _productService.Create(dto);

        return CreatedAtRoute(
            "GetProductById",
            new { id = product.Id },
            product);
    }

    // PUT: api/productos/{id}
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        Guid id,
        UpdateProductDto dto)
    {
        var result = await _productService.Update(id, dto);

        if (!result)
            return NotFound();

        return NoContent();
    }

    // DELETE: api/productos/{id}
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _productService.Delete(id);

        if (!result)
            return NotFound();

        return NoContent();
    }
}
