using System.Text.Json.Serialization;

namespace InventarioVentas.API.Modules.Customers.DTOs
{
    public class CustomerResponse
    {
        public Guid Id { get; set; }
        [JsonPropertyName("nombre")]
        public string NombreCompleto { get; set; } = string.Empty;

        [JsonPropertyName("Documento")]
        public int Documento { get; set; }

        [JsonPropertyName("Email")]
        public string Email { get; set; } = string.Empty;
        [JsonPropertyName("Telefono")]
        public string Telefono { get; set; } = string.Empty;

        [JsonPropertyName("FechaRegistro")]
        public DateTime FechaRegistro { get; set; }

    }
}
