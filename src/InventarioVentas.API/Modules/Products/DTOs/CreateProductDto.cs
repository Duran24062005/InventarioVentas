using System.Text.Json.Serialization;

namespace InventarioVentas.API.Modules.Products.DTOs;

public class CreateProductDto
{
    // Product display name (required).
    [JsonPropertyName("nombre")]
    public string Name { get; set; } = string.Empty;

    // Unique SKU or barcode (required).
    [JsonPropertyName("codigo")]
    public string Code { get; set; } = string.Empty;

    // Product sale price. Must be greater than zero.
    [JsonPropertyName("precio")]
    public decimal Price { get; set; }

    // Initial quantity. Must be greater than or equal to zero.
    [JsonPropertyName("stock")]
    public int Stock { get; set; }

    // Category identifier for the product.
    [JsonPropertyName("categoriaId")]
    public int CategoryId { get; set; }
}
