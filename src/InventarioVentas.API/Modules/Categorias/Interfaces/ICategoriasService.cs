using InventarioVentas.API.Modules.Categorias.DTOs;

namespace InventarioVentas.API.Modules.Categorias.Interfaces;

public interface ICategoriasService
{
    Task<IEnumerable<CategoriaReposeDto>> GetAllAsync();

    Task<CategoriaReposeDto?> GetByIdAsync(Guid id);

    Task<CategoriaReposeDto> CreateAsync(
        CrearCategoriaDto dto);

    Task<bool> UpdateAsync(
        Guid id,
        ActualizarCategoriaDto dto);

    Task<bool> DeleteAsync(Guid id);
}