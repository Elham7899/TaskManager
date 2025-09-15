using MediatR;
using TaskManager.Application.DTOs.Login;

namespace TaskManager.Application.Features.Auth.Commands.Login;

public record LoginCommand(LoginDto input) : IRequest<string>
{
    public string UserName { get; set; } = input.UserName;
    public string Password { get; set; } = input.Password;
}