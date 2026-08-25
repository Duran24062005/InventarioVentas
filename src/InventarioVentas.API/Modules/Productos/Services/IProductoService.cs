using InventarioVentas.API.Modules.Productos.DTOs;

namespace InventarioVentas.API.Modules.Productos.Interfaces;

public interface IProductoService
{
    // Método para registrar un producto. Devuelve el DTO con el producto creado.
    Task<ProductoResponseDto> CrearAsync(CrearProductoDto dto);

    // Método para consultar todos los productos (HU03).

    Task<IEnumerable<ProductoResponseDto>> ObtenerTodosAsync();

    // Método para consultar un producto por su ID.
    Task<ProductoResponseDto> ObtenerPorIdAsync(int id);

}