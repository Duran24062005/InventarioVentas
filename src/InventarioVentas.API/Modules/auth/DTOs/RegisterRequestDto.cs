using System.Text.Json.Serialization;

namespace InventarioVentas.API.Modules.auth.DTOs;

public class RegisterRequestDto
{
    [JsonPropertyName("nombre")]
    public required string Name { get; set; } = string.Empty;

    [JsonPropertyName("email")]
    public required string Email { get; set; } = string.Empty;

    [JsonPropertyName("password")]
    public required string Password { get; set; } = string.Empty;
}
