using MediatR;
using TaskManager.Application.DTOs.Common;
using TaskManager.Application.DTOs.Task;

namespace TaskManager.Application.Tasks.Queries;

public record GetTasksQuery(int Page = 1, int PageSize = 20, string? Search = null, bool? IsCompleted = null, int? LabelId = null) : IRequest<ApiResponse<PagedResult<TaskDto>>>;
