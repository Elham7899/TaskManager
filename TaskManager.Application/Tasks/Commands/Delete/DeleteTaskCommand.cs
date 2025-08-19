using MediatR;

namespace TaskManager.Application.Tasks.Commands.Delete;

public record DeleteTaskCommand(int Id) : IRequest<Unit>;
