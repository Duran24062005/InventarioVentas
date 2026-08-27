using System.Text.Json.Serialization;

namespace InventarioVentas.API.Modules.Categories.DTOs
{
    public class UpdateCategoryDto
    {
        [JsonPropertyName("nombre")]
        public required string Name { get; set; } = string.Empty;

        [JsonPropertyName("descripcion")]
        public required string Description { get; set; } = string.Empty;

        [JsonPropertyName("estado")]
        public required bool IsActive { get; set; }

    }
}
