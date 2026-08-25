using InventarioVentas.API.Modules.Categories.DTOs;

namespace InventarioVentas.API.Modules.Categories.Interfaces;

public interface ICategoryService
{
    Task<IEnumerable<CategoryResponseDto>> GetAll();

    Task<CategoryResponseDto?> GetById(Guid id);

    Task<CategoryResponseDto> Create(
        CreateCategoryDto dto);

    Task<bool> Update(
        Guid id,
        UpdateCategoryDto dto);

    Task<bool> Delete(Guid id);
}
