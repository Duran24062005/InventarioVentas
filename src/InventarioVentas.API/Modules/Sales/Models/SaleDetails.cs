namespace InventarioVentas.API.Modules.Sales.Models
{
    public class SaleDetails
    {
        public Guid Id { get; set;  }
        public Guid SaleId { get; set; }
        public Sale Sale { get; set; } = null!;


    }
}
