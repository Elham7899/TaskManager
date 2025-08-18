using MediatR;
using TaskManager.Application.DTOs.Task;

namespace TaskManager.Application.Tasks.Queries.GetBy;

public record GetTaskByIdQuery(int Id) : IRequest<TaskDto?>;
