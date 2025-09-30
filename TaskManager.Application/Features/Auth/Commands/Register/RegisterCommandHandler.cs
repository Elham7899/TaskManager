using MediatR;
using Microsoft.AspNetCore.Identity;
using TaskManager.Application.DTOs.Login;
using TaskManager.Application.Features.Auth.Commands.Login;
using TaskManager.Domain.Entities;

namespace TaskManager.Application.Features.Auth.Commands.Register;

public class RegisterCommandHandler(UserManager<ApplicationUser> userManager, IMediator mediator) : IRequestHandler<RegisterCommand, string>
{
    public async Task<string> Handle(RegisterCommand request, CancellationToken cancellationToken)
     {
        var user = new ApplicationUser
        {
            UserName = request.UserName,
            Email = request.Email
        };

        var result = await userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
            throw new Exception(string.Join("; ", result.Errors.Select(e => e.Description)));

        await userManager.AddToRoleAsync(user, "User");

        var input = new LoginDto { UserName = request.UserName, Password = request.Password };

        return await mediator.Send(new LoginCommand(input), cancellationToken);
    }
}