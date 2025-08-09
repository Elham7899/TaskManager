using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManager.API.Responses;
using TaskManager.Application.DTOs.Task;
using TaskManager.Application.Interfaces;
using TaskManager.Domain.Entities;

namespace TaskManager.API.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class TasksController(ITaskService taskService, IMapper mapper) : ControllerBase
{
    /// <summary>
    /// Gets all tasks with optional title filtering.
    /// </summary>
    /// <param name="title">Filter by title</param>
    /// <param name="page">Page number</param>
    /// <param name="pageSize">Page size</param>
    /// <returns>List of tasks</returns>
    [HttpGet]
    [Authorize(Roles = "User")]
    [ProducesResponseType(typeof(ApiResponse<List<TaskDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<List<TaskDto>>>> GetAll(bool isComplete, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var tasks = await taskService.GetAllTasksAsync(isComplete, page, pageSize);
        var dtos = mapper.Map<List<TaskDto>>(tasks);
        return Ok(ApiResponse<List<TaskDto>>.ReturnSuccess(dtos, new PaginationMetadata(page, pageSize, dtos.Count)));
    }

    /// <summary>
    /// Create task with optional title.
    /// </summary>
    /// <returns>List of tasks</returns>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ApiResponse<TaskDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<TaskDto>>> Create([FromBody] CreateTaskDto input)
    {
        var task = mapper.Map<TaskItem>(input);
        var result = await taskService.AddTaskAsync(task, input.LabelIds);
        return Ok(ApiResponse<TaskDto>.ReturnSuccess(mapper.Map<TaskDto>(task)));
    }

    /// <summary>
    /// Gets task by Id.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    [Authorize(Roles = "User")]
    public async Task<ActionResult<ApiResponse<TaskDto>>> GetTaskById(int id)
    {
        var task = await taskService.GetTaskByIdAsync(id);
        if (task == null)
            return NotFound();

        return Ok(ApiResponse<TaskDto>.ReturnSuccess(mapper.Map<TaskDto>(task)));
    }
}
