using Microsoft.AspNetCore.Mvc;
using TaskManager.Application.DTOs.Login;
using TaskManager.Application.Interfaces;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IAuthService authService, IRoleService roleService) : ControllerBase
{
    /// <summary>
    /// Login an existing user
    /// </summary>
    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        var token = await authService.LoginAsync(dto);
        return Ok(new { token });
    }

    /// <summary>
    /// Register a new user
    /// </summary>
    [HttpPost("register")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register([FromBody] RegisterDto dto)
    {
        var token = await authService.RegisterAsync(dto);
        return Ok(new { token });
    }

    [HttpPost("assign")]
    public async Task<IActionResult> AssignRole([FromBody] AssignRoleRequestDto request)
    {
        try
        {
            var result = await roleService.AssignRoleToUserAsync(request.UserId, request.Role);
            if (result)
                return Ok(new { message = "Role assigned successfully" });

            return BadRequest("Failed to assign role");
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }
}
