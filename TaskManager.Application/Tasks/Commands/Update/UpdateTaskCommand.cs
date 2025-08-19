using MediatR;
using TaskManager.Application.DTOs.Task;

namespace TaskManager.Application.Tasks.Commands.Update;

public record UpdateTaskCommand(UpdateTaskDto Dto, string CurrentUserId) : IRequest<TaskDto>;
