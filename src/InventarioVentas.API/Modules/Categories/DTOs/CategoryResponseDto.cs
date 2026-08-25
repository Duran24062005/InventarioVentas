using System.Text.Json.Serialization;

namespace InventarioVentas.API.Modules.Categories.DTOs
{
    public class CategoryResponseDto
    {
        public Guid Id { get; set; }
        [JsonPropertyName("nombre")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("descripcion")]
        public string Description { get; set; } = string.Empty;

        [JsonPropertyName("fechaCreacion")]
        public DateTime CreatedAt { get; set; }

        [JsonPropertyName("estado")]
        public bool IsActive { get; set; }
    }
}
