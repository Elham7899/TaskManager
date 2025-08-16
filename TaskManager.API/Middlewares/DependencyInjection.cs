using FluentValidation;
using MediatR;
using System.Reflection;
using TaskManager.Application.Behaviors;

namespace TaskManager.API.Middlewares;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var appAssembly = Assembly.Load("TaskManager.Application");

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(appAssembly));
        services.AddAutoMapper(appAssembly);
        services.AddValidatorsFromAssembly(appAssembly);

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        return services;
    }
}