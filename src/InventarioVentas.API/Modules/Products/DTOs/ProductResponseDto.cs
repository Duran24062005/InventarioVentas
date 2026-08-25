using System.Text.Json.Serialization;

namespace InventarioVentas.API.Modules.Products.DTOs;

public class ProductResponseDto
{
    public int Id { get; set; }

    [JsonPropertyName("nombre")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("codigo")]
    public string Code { get; set; } = string.Empty;

    [JsonPropertyName("precio")]
    public decimal Price { get; set; }

    [JsonPropertyName("stock")]
    public int Stock { get; set; }

    [JsonPropertyName("estado")]
    public bool IsActive { get; set; }

    // Basic information about the linked category.
    [JsonPropertyName("categoriaId")]
    public int CategoryId { get; set; }

    [JsonPropertyName("nombreCtegoria")]
    public string CategoryName { get; set; } = string.Empty;
}
