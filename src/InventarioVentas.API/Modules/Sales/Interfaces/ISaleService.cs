using InventarioVentas.API.Modules.Sales.DTOs;

namespace InventarioVentas.API.Modules.Sales.Interfaces
{

    public interface ISaleService
    {
        Task<IEnumerable<SaleResponseDto>> GetAll();

        Task<SaleResponseDto> Create(CreateSaleDto dto);

        Task<SaleResponseDto?> GetById(Guid id);

        
    } 


}