using InventarioVentas.API.Modules.Products.Models;
namespace InventarioVentas.API.Modules.Sales.Models
{
    public class SaleDetails
    {
        public Guid Id { get; set;  }
        public Guid SaleId { get; set; }
        public Sale Sale { get; set; } = null!;

        public Guid ProductId { get; set; }
        public Product Product { get; set; } = null!;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Subtotal { get; set; }


    }
}
