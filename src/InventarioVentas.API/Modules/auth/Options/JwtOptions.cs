using System.Text;

namespace InventarioVentas.API.Modules.auth.Options;

public sealed class JwtOptions
{
    public const string SectionName = "Jwt";

    public string Key { get; set; } = string.Empty;
    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
    public int ExpiresMinutes { get; set; } = 60;

    public bool IsValid =>
        Encoding.UTF8.GetByteCount(Key) >= 32 &&
        !string.IsNullOrWhiteSpace(Issuer) &&
        !string.IsNullOrWhiteSpace(Audience) &&
        ExpiresMinutes > 0;
}
