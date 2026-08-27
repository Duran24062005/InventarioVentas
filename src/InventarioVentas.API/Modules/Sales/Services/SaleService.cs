using InventarioVentas.API.Data;
using InventarioVentas.API.Modules.Sales.DTOs;
using InventarioVentas.API.Modules.Sales.Interfaces;
using InventarioVentas.API.Modules.Sales.Models;
using InventarioVentas.API.Common.Exceptions;
using Microsoft.EntityFrameworkCore;
using InventarioVentas.API.Modules.Products.Models;

namespace InventarioVentas.API.Modules.Sales.Services;


public class SaleService : ISaleService
{

    private readonly AppDbContext _context;

    public SaleService(AppDbContext context)
    {
         _context = context;
    }



// ============================================================
// CREATE
// ============================================================

    public async Task<SaleResponseDto> Create(CreateSaleDto dto)
    {
        if (dto.Details is null || dto.Details.Count == 0)
        {
            throw new ValidationException(
                "La venta debe tener al menos un detalle.");
        }

        var customerExists = await _context.Customers
            .AnyAsync(customer => customer.Id == dto.CustomerId);

        if (!customerExists)
        {
            throw new NotFoundException(
                "El cliente no existe.");
        }

        var productIds = dto.Details
            .Select(d => d.ProductId)
            .Distinct()
            .ToList();

        var products = await _context.Products
            .Where(product => productIds.Contains(product.Id))
            .ToDictionaryAsync(product => product.Id);


        foreach (var detail in dto.Details)
        {
            if (detail.Quantity <= 0)
            {
                throw new ValidationException(
                    "La cantidad debe ser mayor a cero.");
            }

            if (!products.TryGetValue(detail.ProductId, out var product))
            {
                throw new NotFoundException(
                    "El producto no existe.");
            }

            if (!product.IsActive)
            {
                throw new BusinessException(
                    "El producto está inactivo.");
            }

            if (product.Stock < detail.Quantity)
            {
                throw new BusinessException(
                    "Stock insuficiente para el producto.");
            }
        }

      
        await using var transaction =
            await _context.Database.BeginTransactionAsync();

            try
            {
                var sale = new Sale
                {
                    Id = Guid.NewGuid(),
                    CustomerId = dto.CustomerId,
                    SaleDate = DateTime.UtcNow,
                    Details = new List<SaleDetails>()
                };

                foreach (var detail in dto.Details)
                {
                    var product = products[detail.ProductId];
                    var subtotal = detail.Quantity * product.Price;

                    product.Stock -= detail.Quantity;

                    sale.Details.Add(new SaleDetails
                    {
                        Id = Guid.NewGuid(),
                        ProductId = product.Id,
                        Quantity = detail.Quantity,
                        UnitPrice = product.Price,
                        Subtotal = subtotal
                    });
                }

                sale.Total = sale.Details.Sum(detail => detail.Subtotal);

                _context.Sales.Add(sale);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return new SaleResponseDto
                {
                    Id = sale.Id,
                    CustomerId = sale.CustomerId,
                    SaleDate = sale.SaleDate,
                    Total = sale.Total,

                    Details = sale.Details.Select(detail => new SaleDetailResponseDto
                    {
                        ProductId = detail.ProductId,
                        Quantity = detail.Quantity,
                        UnitPrice = detail.UnitPrice,
                        Subtotal = detail.Subtotal
                    }).ToList()
                };
        }


            catch 
            {
                await transaction.RollbackAsync();
                throw;
            }

    } 


    // ============================================================
    // GET ALL
    // ============================================================


    public async Task<IEnumerable<SaleResponseDto>> GetAll()
    {
        var sales = await _context.Sales
            .Include(sale => sale.Details)
            .ToListAsync();
        
        return sales.Select(sale => new SaleResponseDto
        {
            Id = sale.Id,
            CustomerId = sale.CustomerId,
            SaleDate = sale.SaleDate,
            Total = sale.Total,

            Details = sale.Details.Select(detail => new SaleDetailResponseDto
            {
                ProductId = detail.ProductId,
                Quantity = detail.Quantity,
                UnitPrice = detail.UnitPrice,
                Subtotal = detail.Subtotal
            }).ToList()
        });
    }

    



    public async Task<SaleResponseDto?> GetById(Guid id)
    {
        var sale = await _context.Sales
            .Include(sale => sale.Details)
            .FirstOrDefaultAsync(sale => sale.Id == id);

            if (sale is null)
                {
                    return null;
                }
        
        return new SaleResponseDto
        {
            Id = sale.Id,
            CustomerId = sale.CustomerId,
            SaleDate = sale.SaleDate,
            Total = sale.Total,

            Details = sale.Details.Select(detail => new SaleDetailResponseDto
            {
                ProductId = detail.ProductId,
                Quantity = detail.Quantity,
                UnitPrice = detail.UnitPrice,
                Subtotal = detail.Subtotal

            }).ToList()
        };
    }




}