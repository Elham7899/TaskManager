using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManager.API.DTOs;
using TaskManager.Application.DTOs;
using TaskManager.Application.Interfaces;

namespace TaskManager.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LabelsController(ILabelService labelService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create(CreateLabelDto input)
    {
        var label = await labelService.CreateLabelAsync(input);
        return Ok(label);
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var labels = await labelService.GetAllLabelsAsync();
        return Ok(labels);
    }

    [Authorize]
    [HttpPost("{taskId}/assign")]
    public async Task<IActionResult> AssignLabelToTask(int taskId, [FromBody] AssignLabelsDto input)
    {
        await labelService.AssignLabelsToTaskAsync(taskId, input.LabelIds);
        return Ok();
    }

    [Authorize]
    [HttpDelete("{taskId}/remove/{labelId}")]
    public async Task<IActionResult> RemoveLabelFromTask(int taskId, int labelId)
    {
        await labelService.RemoveLabelsFromTaskAsync(taskId, labelId);
        return Ok();
    }

    [Authorize]
    [HttpPut("{labelId}")]
    public async Task<IActionResult> UpdateLabel(int labelId, [FromBody] string newName)
    {
        var label = await labelService.UpdateLabelAsync(labelId, newName);
        return Ok(label);
    }

    [Authorize]
    [HttpDelete("{labelId}")]
    public async Task<IActionResult> DeleteLabel(int labelId)
    {
        await labelService.DeleteLabelAsync(labelId);
        return Ok();
    }
}
