using InventarioVentas.API.Modules.Customers.DTOs;

namespace InventarioVentas.API.Modules.Customers.Interfaces;

public interface ICustomerService
{
    Task<IEnumerable<CustomerResponse>> GetAll();
    Task<CustomerResponse?> GetById(Guid id);
    Task<CustomerResponse> Create(CreateCustomerDto dto);
    Task<bool> Update(Guid id, UpdateCustomerDto dto);
    Task<bool> Delete(Guid id);
}