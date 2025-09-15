using MediatR;
using TaskManager.Application.DTOs.Login;

namespace TaskManager.Application.Features.Auth.Commands.AssignRole;

public record AssignRoleCommand(AssignRoleRequestDto input) : IRequest<bool>
{
    public string UserId { get; set; } = input.UserId;
    public string Role { get; set; } = input.Role;
}
