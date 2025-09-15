using MediatR;

namespace TaskManager.Application.Features.Tasks.Commands.Delete;

public record DeleteTaskCommand(int Id) : IRequest<Unit>;
