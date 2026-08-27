using InventarioVentas.API.Modules.Sales.Interfaces;
using InventarioVentas.API.Modules.Sales.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace InventarioVentas.API.Modules.Sales.Controllers;

[ApiController]
[Route("/api/sales")]
public class SalesController : ControllerBase
{
    private readonly ISaleService _saleService;

     public SalesController(ISaleService saleService)
    {
        _saleService = saleService;
    }

    // GET: api/Sale

    [HttpGet]
    public async Task<ActionResult<IEnumerable<SaleResponseDto>>> GetAllAsync()
    {
        var sale = await _saleService.GetAll();

        return Ok(sale);
    }

    // GET: api/Sale/{id}
    [HttpGet("{id:guid}", Name = "GetSaleById")]
    public async Task<ActionResult<SaleResponseDto>> GetByIdAsync(Guid id)
    {
        var sale = await _saleService.GetById(id);

        if (sale is null)
            return NotFound();

        return Ok(sale);
    }


    // POST: api/Sale
    [HttpPost]
    public async Task<ActionResult<SaleResponseDto>> CreateAsync(
        CreateSaleDto dto)
    {
        var sale = await _saleService.Create(dto);

            return CreatedAtRoute(
                "GetSaleById",
                new { id = sale.Id },
                sale);
    }

}