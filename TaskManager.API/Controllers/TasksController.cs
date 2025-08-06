using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
    public async Task<IActionResult> GetAll(bool isComplete, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var tasks = await taskService.GetAllTasksAsync(isComplete, page, pageSize);
        return Ok(tasks);
    }

    /// <summary>
    /// Create task with optional title.
    /// </summary>
    /// <returns>List of tasks</returns>
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create([FromBody] CreateTaskDto input)
    {
        var task = mapper.Map<TaskItem>(input);
        var result = await taskService.AddTaskAsync(task, input.LabelIds);
        return CreatedAtAction(nameof(GetTaskById), new { id = task.Id }, mapper.Map<TaskDto>(task));
    }

    /// <summary>
    /// Gets task by Id.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    [Authorize(Roles = "User")]
    public async Task<IActionResult> GetTaskById(int id)
    {
        var task = await taskService.GetTaskByIdAsync(id);
        if (task == null)
            return NotFound();

        return Ok(mapper.Map<TaskDto>(task));
    }
}
