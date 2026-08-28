using InventarioVentas.API.Modules.auth.DTOs;
using InventarioVentas.API.Modules.auth.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace InventarioVentas.API.Modules.auth.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        // POST: api/auth/login
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDto>> Login(
            LoginRequestDto dto)
        {
            var response = await _authService.LoginAsync(dto);

            return Ok(response);
        }

        // POST: api/auth/register
        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult<AuthResponseDto>> Register(RegisterRequestDto dto)
        {

            var user = await _authService.CreateUserAsync(dto);

            return StatusCode(StatusCodes.Status201Created, new AuthResponseDto
            {
                Token = string.Empty,
                Id = user.Id,
                Name = user.Name,
                Email = user.Email ?? string.Empty
            });
        }

        [Authorize]
        [HttpGet("me")]
        public ActionResult<UserResponseDto> Me()
        {
            return Ok(new UserResponseDto
            {
                Id = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!),
                Name = User.FindFirstValue(ClaimTypes.Name) ?? string.Empty,
                Email = User.FindFirstValue(ClaimTypes.Email) ?? string.Empty
            });
        }
    }

}
