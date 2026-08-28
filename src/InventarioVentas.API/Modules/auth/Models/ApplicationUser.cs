using Microsoft.AspNetCore.Identity;

namespace InventarioVentas.API.Modules.auth.Models;

public class ApplicationUser : IdentityUser<Guid>
{
    public string Name { get; set; } = string.Empty;

}
