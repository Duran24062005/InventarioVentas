using InventarioVentas.API.Modules.Customers.Models;

namespace InventarioVentas.API.Modules.Sales.Models
{
    public class Sale
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public CustomerModel Customer { get; set; } = null!;
        public DateTime SaleDate {  get; set; }
        public decimal Total {  get; set; }

       /// public ICollection<DetalleVenta> Detalles { get; set; } = new List<DetalleVenta>();

    }
}
