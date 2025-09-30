using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Application.DTOs.Common;
using TaskManager.Application.DTOs.Task;
using TaskManager.Application.Features.Tasks.Commands.Create;
using TaskManager.Application.Features.Tasks.Queries.GetAll;
using TaskManager.Application.Features.Tasks.Queries.GetBy;

namespace TaskManager.API.Controllers;

[ApiController]
//[Authorize]
[Route("api/[controller]")]
public class TasksController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Gets all tasks with optional title filtering.
    /// </summary>
    /// <param name="title">Filter by title</param>
    /// <param name="page">Page number</param>
    /// <param name="pageSize">Page size</param>
    /// <returns>List of tasks</returns>
    [HttpGet]
    //[Authorize(Roles = "User")]
    [ProducesResponseType(typeof(ApiResponse<List<TaskDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<List<TaskDto>>>> GetAll(
        bool? isCompleted,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        var result = await mediator.Send(new GetTasksQuery(page, pageSize, null, isCompleted));
        return Ok(result);
    }

    /// <summary>
    /// Create task with optional title.
    /// </summary>
    /// <returns>List of tasks</returns>
    [HttpPost]
    [Authorize(Roles = "Admin,User")]
    [ProducesResponseType(typeof(ApiResponse<TaskDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<TaskDto>>> Create([FromBody] CreateTaskDto input)
    {
        var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        var result = await mediator.Send(new CreateTaskCommand(input, userId!));

        return Ok(ApiResponse<TaskDto>.ReturnSuccess(result));
    }

    /// <summary>
    /// Gets task by Id.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    [Authorize(Roles = "Admin,User")]
    [ProducesResponseType(typeof(ApiResponse<TaskDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<TaskDto>>> GetById(int id)
    {
        var task = await mediator.Send(new GetTaskByIdQuery(id));
        return task is null
            ? NotFound()
            : Ok(task);
    }
}
