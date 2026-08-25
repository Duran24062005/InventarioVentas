using InventarioVentas.API.Modules.Customers.DTOs;

namespace InventarioVentas.API.Modules.Customers.Interfaces;

public interface ICustomerService
{
    Task<IEnumerable<CustomerResponse>> GetAllAsync();
    Task<CustomerResponse?> GetByIdAsync(Guid id);
    Task<CustomerResponse> CreateAsync(CreateCustomerDto dto);
    Task<bool> UpdateAsync(Guid id, UpdateCustomerDto dto);
    Task<bool> DeleteAsync(Guid id);
}