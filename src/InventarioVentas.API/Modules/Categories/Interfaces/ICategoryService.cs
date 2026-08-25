using InventarioVentas.API.Modules.Categories.DTOs;

namespace InventarioVentas.API.Modules.Categories.Interfaces;

public interface ICategoryService
{
    Task<IEnumerable<CategoryResponseDto>> GetAllAsync();

    Task<CategoryResponseDto?> GetByIdAsync(Guid id);

    Task<CategoryResponseDto> CreateAsync(
        CreateCategoryDto dto);

    Task<bool> UpdateAsync(
        Guid id,
        UpdateCategoryDto dto);

    Task<bool> DeleteAsync(Guid id);
}
