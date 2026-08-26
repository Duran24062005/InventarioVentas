using InventarioVentas.API.Modules.Products.DTOs;

namespace InventarioVentas.API.Modules.Products.Interfaces
{
    public interface IProductService

    {
        Task<IEnumerable<ProductResponseDto>> GetAll();

        Task<ProductResponseDto?> GetById(Guid id);

        Task<ProductResponseDto> Create(CreateProductDto dto);

        Task<bool> Update(Guid id, UpdateProductDto dto);
        Task<bool> Delete(Guid id);

    }
}
