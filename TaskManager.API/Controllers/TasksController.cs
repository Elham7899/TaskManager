using Microsoft.AspNetCore.Mvc;
using TaskManager.Application.Interfaces;
using TaskManager.Domain.Entities;

namespace TaskManager.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TasksController : ControllerBase
{
    private readonly ITaskService _taskService;
    public TasksController(ITaskService taskService)
    {
        _taskService = taskService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var tasks = await _taskService.GetAllTasksAsync();
        return Ok(tasks);
    }

    [HttpPost]
    public async Task<IActionResult> Create(TaskItem task)
    {
        var result = await _taskService.AddTaskAsync(task);
        return CreatedAtAction(nameof(GetAll), new { id = result.Id }, result);
    }
}
