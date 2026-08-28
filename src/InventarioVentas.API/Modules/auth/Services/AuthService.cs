using InventarioVentas.API.Modules.auth.Models;
using Microsoft.AspNetCore.Identity;
using InventarioVentas.API.Common.Exceptions;
using InventarioVentas.API.Modules.auth.DTOs;
using InventarioVentas.API.Modules.auth.Interfaces;

namespace InventarioVentas.API.Modules.auth.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ITokenService _tokenService;

    public AuthService(UserManager<ApplicationUser> userManager, ITokenService tokenService)
    {
        _userManager = userManager;
        _tokenService = tokenService;
    }

    public async Task<ApplicationUser?> GetUserByEmailAsync(string email)
    {
        return await _userManager.FindByEmailAsync(email);
    }

    public async Task<AuthResponseDto> LoginAsync(LoginRequestDto dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email);

        if (user == null || !await _userManager.CheckPasswordAsync(user, dto.Password))
        {
            throw new UnauthorizedException(
                "El email o la contraseña no son válidos.");
        }

        return _tokenService.CreateToken(user);
    }

    public async Task<ApplicationUser> CreateUserAsync(RegisterRequestDto dto)
    {
        var user = new ApplicationUser
        {
            Id = Guid.NewGuid(),
            UserName = dto.Email,
            Email = dto.Email,
            Name = dto.Name
        };

        var result = await _userManager.CreateAsync(user, dto.Password);

        if (!result.Succeeded)
        {
            var errors = result.Errors
                .Select(error => error.Description);

            throw new ValidationException(string.Join(", ", errors));
        }

        return user;
    }

}
