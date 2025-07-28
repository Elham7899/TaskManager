using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManager.API.DTOs;
using TaskManager.Application.DTOs.Label;
using TaskManager.Application.Interfaces;

namespace TaskManager.API.Controllers;

/// <summary>
/// کنترلر تگ ها
/// </summary>
/// <param name="labelService"></param>
/// <param name="mapper"></param>
[Route("api/[controller]")]
[Authorize]
[ApiController]
public class LabelsController(ILabelService labelService, IMapper mapper) : ControllerBase
{
    /// <summary>
    /// Create Label
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<IActionResult> Create(CreateLabelDto input)
    {
        var label = await labelService.CreateLabelAsync(input);
        var dto = mapper.Map<LabelDto>(label);
        return Ok(dto);
    }

    /// <summary>
    /// Gets all Labels.
    /// </summary>
    /// <param name="page"></param>
    /// <param name="pageSize"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var labels = await labelService.GetAllLabelsAsync(page, pageSize);
        var dtos = mapper.Map<List<LabelDto>>(labels);
        return Ok(dtos);
    }

    /// <summary>
    /// Assign Label To Task
    /// </summary>
    /// <param name="taskId"></param>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpPost("{taskId}/assign")]
    public async Task<IActionResult> AssignLabelToTask(int taskId, [FromBody] AssignLabelsDto input)
    {
        await labelService.AssignLabelsToTaskAsync(taskId, input.LabelIds);
        return Ok();
    }

    /// <summary>
    /// Remove Label From Task
    /// </summary>
    /// <param name="taskId"></param>
    /// <param name="labelId"></param>
    /// <returns></returns>
    [HttpDelete("{taskId}/remove/{labelId}")]
    public async Task<IActionResult> RemoveLabelFromTask(int taskId, int labelId)
    {
        await labelService.RemoveLabelsFromTaskAsync(taskId, labelId);
        return Ok();
    }

    /// <summary>
    /// Edit Label
    /// </summary>
    /// <param name="labelId"></param>
    /// <param name="newName"></param>
    /// <returns></returns>
    [HttpPut("{labelId}")]
    public async Task<IActionResult> UpdateLabel(int labelId, [FromBody] string newName)
    {
        var updatedLabel = await labelService.UpdateLabelAsync(labelId, newName);
        var dto = mapper.Map<LabelDto>(updatedLabel);
        return Ok(dto);
    }

    /// <summary>
    /// Delete Label
    /// </summary>
    /// <param name="labelId"></param>
    /// <returns></returns>
    [HttpDelete("{labelId}")]
    public async Task<IActionResult> DeleteLabel(int labelId)
    {
        await labelService.DeleteLabelAsync(labelId);
        return Ok();
    }
}
