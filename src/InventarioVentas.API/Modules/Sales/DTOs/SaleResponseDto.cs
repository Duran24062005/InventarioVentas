using System.Text.Json.Serialization;
namespace InventarioVentas.API.Modules.Sales.DTOs;

public class SaleResponseDto 
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [JsonPropertyName("clienteId")]
    public Guid CustomerId { get; set; }

    [JsonPropertyName("fechaVenta")]
    public DateTime SaleDate { get; set; }

    [JsonPropertyName("total")]
    public decimal Total { get; set; }


    [JsonPropertyName("detalles")]

    public List<SaleDetailResponseDto> Details { get; set; } = new();
}

public class SaleDetailResponseDto 
{
    [JsonPropertyName("productoId")]
    public Guid ProductId { get; set; }

    [JsonPropertyName("cantidad")]
    public int Quantity { get; set; }

    [JsonPropertyName("precioUnitario")]
    public decimal UnitPrice { get; set; }

    [JsonPropertyName("subtotal")]
    public decimal Subtotal { get; set; }


}

