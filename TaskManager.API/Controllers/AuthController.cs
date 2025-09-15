using MediatR;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Application.DTOs.Login;
using TaskManager.Application.Features.Auth.Commands.AssignRole;
using TaskManager.Application.Features.Auth.Commands.Login;
using TaskManager.Application.Features.Auth.Commands.Register;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Login an existing user
    /// </summary>
    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        var token = await mediator.Send(new LoginCommand(dto));
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
        var token = await mediator.Send(new RegisterCommand(dto));
        return Ok(new { token });
    }

    [HttpPost("assign")]
    public async Task<IActionResult> AssignRole([FromBody] AssignRoleRequestDto request)
    {
        try
        {
            var result = await mediator.Send(new AssignRoleCommand(request));
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
