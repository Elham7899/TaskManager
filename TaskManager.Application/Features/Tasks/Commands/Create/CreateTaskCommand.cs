using MediatR;
using TaskManager.Application.DTOs.Task;

namespace TaskManager.Application.Features.Tasks.Commands.Create;

// Command
public record   CreateTaskCommand(CreateTaskDto Dto, string CurrentUserId) : IRequest<TaskDto>;
