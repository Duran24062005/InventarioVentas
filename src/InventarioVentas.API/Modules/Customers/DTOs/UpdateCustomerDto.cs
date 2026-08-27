using System.Text.Json.Serialization;

namespace InventarioVentas.API.Modules.Customers.DTOs
{
    public class UpdateCustomerDto
    {
        [JsonPropertyName("NombreCompleto")]
        public required string NombreCompleto { get; set; } = string.Empty;
        [JsonPropertyName("Documento")]
        public required int Documento { get; set; }
        [JsonPropertyName("Email")]
        public required string Email { get; set; } = string.Empty;
        [JsonPropertyName("Telefono")]
        public required string Telefono { get; set; } = string.Empty;
    }
}
