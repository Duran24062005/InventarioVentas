
using InventarioVentas.API.Modules.auth.DTOs;
using InventarioVentas.API.Modules.auth.Models;

namespace InventarioVentas.API.Modules.auth.Interfaces;

public interface ITokenService
{
    AuthResponseDto CreateToken(ApplicationUser user);
}
