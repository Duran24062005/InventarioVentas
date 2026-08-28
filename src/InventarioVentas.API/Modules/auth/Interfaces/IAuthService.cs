using InventarioVentas.API.Modules.auth.DTOs;
using InventarioVentas.API.Modules.auth.Models;

namespace InventarioVentas.API.Modules.auth.Interfaces;

public interface IAuthService
{
    Task<ApplicationUser?> GetUserByEmailAsync(string email);

    Task<AuthResponseDto> LoginAsync(LoginRequestDto dto);

    Task<ApplicationUser> CreateUserAsync(RegisterRequestDto dto);

}
