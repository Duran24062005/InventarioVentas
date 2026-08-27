using System.Text.Json.Serialization;

namespace InventarioVentas.API.Modules.Categories.DTOs
{
    public class CreateCategoryDto
    {
        [JsonPropertyName("nombre")]
        public required string Name { get; set; } = string.Empty;

        [JsonPropertyName("descripcion")]
        public required string Description { get; set; } = string.Empty;

    }
}
