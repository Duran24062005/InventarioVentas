using InventarioVentas.API.Modules.Customers.DTOs;
using InventarioVentas.API.Modules.Customers.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace InventarioVentas.API.Modules.Customers.Controllers;

[ApiController]
[Route("api/clientes")]
public class CustomersController : ControllerBase
{
    private readonly ICustomerService _customerService;

    public CustomersController(
        ICustomerService customerService)
    {
        _customerService = customerService;
    }

    // GET: api/clientes
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CustomerResponse>>> GetAll()
    {
        var customers = await _customerService.GetAll();

        return Ok(customers);
    }

    // GET: api/clientes/{id}
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<CustomerResponse>> GetById(Guid id)
    {
        var customer = await _customerService.GetById(id);

        if (customer is null)
            return NotFound();

        return Ok(customer);
    }

    // POST: api/clientes
    [HttpPost]
    public async Task<ActionResult<CustomerResponse>> Create(
        CreateCustomerDto dto)
    {
        var customer = await _customerService.Create(dto);

        return CreatedAtAction(
            nameof(GetById),
            new { id = customer.Id },
            customer);
    }

    // PUT: api/clientes/{id}
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        Guid id,
        UpdateCustomerDto dto)
    {
        var result = await _customerService.Update(id, dto);

        if (!result)
            return NotFound();

        return NoContent();
    }

    // DELETE: api/clientes/{id}
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _customerService.Delete(id);

        if (!result)
            return NotFound();

        return NoContent();
    }
}
