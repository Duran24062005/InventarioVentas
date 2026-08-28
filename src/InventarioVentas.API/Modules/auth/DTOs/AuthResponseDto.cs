using System.Text.Json.Serialization;

namespace InventarioVentas.API.Modules.auth.DTOs;

public class AuthResponseDto
{
    [JsonPropertyName("token")]
    public required string Token { get; set; } = string.Empty;

    [JsonPropertyName("expiresAt")]
    public DateTime ExpiresAt { get; set; }

    [JsonPropertyName("id")]
    public required Guid Id { get; set; }

    [JsonPropertyName("nombre")]
    public required string Name { get; set; } = string.Empty;

    [JsonPropertyName("email")]
    public required string Email { get; set; } = string.Empty;

}
