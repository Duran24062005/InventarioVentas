using InventarioVentas.API.Modules.Products.DTOs;

namespace InventarioVentas.API.Modules.Products.Interfaces;

public interface IProductService
{
    // Creates a product and returns its response DTO.
    Task<ProductResponseDto> CreateAsync(CreateProductDto dto);

    // Returns all products (HU03).
    Task<IEnumerable<ProductResponseDto>> GetAllAsync();

    // Returns a product by its identifier.
    Task<ProductResponseDto> GetByIdAsync(int id);

}
