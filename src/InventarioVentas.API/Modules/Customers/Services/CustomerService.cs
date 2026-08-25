using InventarioVentas.API.Data.Configurations;
using InventarioVentas.API.Modules.Customers.DTOs;
using InventarioVentas.API.Modules.Customers.Interfaces;
using InventarioVentas.API.Modules.Customers.Models;
using Microsoft.EntityFrameworkCore;


namespace InventarioVentas.API.Modules.Customers.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly CustomerDbContext _context;

        public CustomerService(CustomerDbContext context)
        {
            _context = context;
        }



        ///GET ALL


        public async Task<IEnumerable<CustomerResponse>> GetAllAsync()
        {
            return await _context.Customers
                .Select(cus => new CustomerResponse
                {
                    Id = cus.Id,
                    NombreCompleto = cus.NombreCompleto,
                    Documento = cus.Documento,
                    Email = cus.Email,
                    Telefono = cus.Telefono,
                    FechaRegistro = cus.FechaRegistro,
                })
                .ToListAsync();



        }

        //GET BY ID

        public async Task<CustomerResponse?> GetByIdAsync(Guid id)
        {
            var customer = await _context.Customers
                .FirstOrDefaultAsync(cus => cus.Id == id);
            if (customer is null)
                return null;

            return new CustomerResponse
            {
                Id = customer.Id,
                NombreCompleto = customer.NombreCompleto,
                Documento = customer.Documento,
                Email = customer.Email,
                Telefono = customer.Telefono,
                FechaRegistro = customer.FechaRegistro,

            };

        }




        ///Create
        ///

        public async Task<CustomerResponse> CreateAsync(
        CreateCustomerDto dto)

        {
            var customer = new CustomerModel
            {
                Id = Guid.NewGuid(),
                NombreCompleto = dto.NombreCompleto,
                Documento = dto.Documento,
                Email = dto.Email,
                Telefono = dto.Telefono,
                FechaRegistro = DateTime.UtcNow
            };

            _context.Customers.Add(customer);

            await _context.SaveChangesAsync();

            return new CustomerResponse
            {
                Id = customer.Id,
                NombreCompleto = customer.NombreCompleto,
                Documento = customer.Documento,
                Email = customer.Email,
                Telefono = customer.Telefono,

            };

        }
        ///UPDATE 
        ///
        public async Task<bool> UpdateAsync(
            Guid id, UpdateCustomerDto dto)
        {
            var customer = await _context.Customers
                .FirstOrDefaultAsync(cus => cus.Id == id);

            if (customer is null)
                return false;

            customer.NombreCompleto = dto.NobreCompleto;
            customer.Documento = dto.Documento;
            customer.Email = dto.Email;
            customer.Telefono = dto.Telefono;

            await _context.SaveChangesAsync();

            return true;

        }
        ///DELETE
        public async Task<bool> DeleteAsync(Guid id)
        {
            var customer = await _context.Customers
                .FirstOrDefaultAsync(cus => cus.Id == id);

            if (customer is null)
                return false;

            _context.Customers.Remove(customer);

            await _context.SaveChangesAsync();

            return true;
        }


















    }
}
