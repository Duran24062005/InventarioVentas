using System.Text.Json.Serialization;

namespace InventarioVentas.API.Modules.Products.DTOs;

public class UpdateProductDto
{
    // Product display name (required).
    [JsonPropertyName("nombre")]
    public required string Name { get; set; } = string.Empty;

    // Unique SKU or barcode (required).
    [JsonPropertyName("codigo")]
    public required string Code { get; set; } = string.Empty;

    // Product sale price. Must be greater than zero.
    [JsonPropertyName("precio")]
    public required decimal Price { get; set; }

    // Initial quantity. Must be greater than or equal to zero.
    [JsonPropertyName("stock")]
    public required int Stock { get; set; }
    [JsonPropertyName("stock")]
    public required DateTime CreatedAt { get; set; }
    [JsonPropertyName("IsActive")]
    public required bool IsActive {  get; set; }
    // Category identifier for the product.
    [JsonPropertyName("categoriaId")]
    public required Guid CategoryId { get; set; }
}
