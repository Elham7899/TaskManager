using MediatR;
using Microsoft.AspNetCore.Identity;
using TaskManager.Domain.Entities;

namespace TaskManager.Application.Features.Auth.Commands.AssignRole;

public class AssignRoleCommandHandler(UserManager<ApplicationUser> userManager) : IRequestHandler<AssignRoleCommand, bool>
{
    public async Task<bool> Handle(AssignRoleCommand request, CancellationToken cancellationToken)
    {
        var user = await userManager.FindByIdAsync(request.UserId);
        if (user == null) return false;

        var result = await userManager.AddToRoleAsync(user, request.Role);
        return result.Succeeded;
    }
}