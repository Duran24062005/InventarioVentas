using System.Text.Json.Serialization;

namespace InventarioVentas.API.Modules.auth.DTOs;

public sealed class UserResponseDto
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [JsonPropertyName("nombre")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("email")]
    public string Email { get; set; } = string.Empty;
}
