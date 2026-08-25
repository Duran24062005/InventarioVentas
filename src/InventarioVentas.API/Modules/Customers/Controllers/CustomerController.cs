using InventarioVentas.API.Modules.Customers.DTOs;
using InventarioVentas.API.Modules.Customers.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace InventarioVentas.API.Modules.Customers.Controllers;

[ApiController]
[Route("api/customers")]
public class CustomersController : ControllerBase
{
    private readonly ICustomerService _customerService;

    public CustomersController(
        ICustomerService customerService)
    {
        _customerService = customerService;
    }

    // GET: api/customers 
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CustomerResponse>>> GetAllAsync()
    {
        var customers = await _customerService.GetAllAsync();

        return Ok(customers);
    }

    // GET: api/customers/{id}
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<CustomerResponse>> GetByIdAsync(Guid id)
    {
        var customer = await _customerService.GetByIdAsync(id);

        if (customer is null)
            return NotFound();

        return Ok(customer);
    }

    // POST: api/customers
    [HttpPost]
    public async Task<ActionResult<CustomerResponse>> CreateAsync(
        CreateCustomerDto dto)
    {
        var customer = await _customerService.CreateAsync(dto);

        return CreatedAtAction(
            nameof(GetByIdAsync),
            new { id = customer.Id },
            customer);
    }

    // PUT: api/customers/{id}
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateAsync(
        Guid id,
        UpdateCustomerDto dto)
    {
        var result = await _customerService.UpdateAsync(id, dto);

        if (!result)
            return NotFound();

        return NoContent();
    }

    // DELETE: api/customers/{id}
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
        var result = await _customerService.DeleteAsync(id);

        if (!result)
            return NotFound();

        return NoContent();
    }
}