using MediatR;
using TaskManager.Application.DTOs.Login;

namespace TaskManager.Application.Features.Auth.Commands.Register;

public record RegisterCommand(RegisterDto input) : IRequest<string>
{
    public string UserName { get; set; } = input.UserName;
    public string Email { get; set; } = input.Email;
    public string Password { get; set; } = input.Password;
}
