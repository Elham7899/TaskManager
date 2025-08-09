using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManager.API.DTOs;
using TaskManager.API.Responses;
using TaskManager.Application.DTOs.Label;
using TaskManager.Application.Interfaces;

namespace TaskManager.API.Controllers;

/// <summary>
/// Manages task labels (create, list, update, delete, assign to tasks)
/// </summary>
[Route("api/[controller]")]
[Authorize(Roles = "User")]
[ApiController]
public class LabelsController(ILabelService labelService, IMapper mapper) : ControllerBase
{
    /// <summary>
    /// Create a new label.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<LabelDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<LabelDto>>> Create([FromBody] CreateLabelDto input)
    {
        var label = await labelService.CreateLabelAsync(input);
        var dto = mapper.Map<LabelDto>(label);
        return Ok(ApiResponse<LabelDto>.ReturnSuccess(dto));
    }

    /// <summary>
    /// Get all labels with pagination.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<List<LabelDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<List<LabelDto>>>> Get([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var labels = await labelService.GetAllLabelsAsync(page, pageSize);
        var dtos = mapper.Map<List<LabelDto>>(labels);
        return Ok(ApiResponse<IEnumerable<LabelDto>>.ReturnSuccess(dtos, new PaginationMetadata(page, pageSize, dtos.Count)));
    }

    /// <summary>
    /// Assign labels to a specific task.
    /// </summary>
    [HttpPost("{taskId}/assign")]
    public async Task<ActionResult<ApiResponse<string>>> AssignLabelToTask(int taskId, [FromBody] AssignLabelsDto input)
    {
        await labelService.AssignLabelsToTaskAsync(taskId, input.LabelIds);
        return Ok(ApiResponse<string>.ReturnSuccess("Labels assigned successfully."));
    }

    /// <summary>
    /// Remove a label from a task.
    /// </summary>
    [HttpDelete("{taskId}/remove/{labelId}")]
    public async Task<ActionResult<ApiResponse<string>>> RemoveLabelFromTask(int taskId, int labelId)
    {
        await labelService.RemoveLabelsFromTaskAsync(taskId, labelId);
        return Ok(ApiResponse<string>.ReturnSuccess("Label removed successfully."));
    }

    /// <summary>
    /// Update a label's name.
    /// </summary>
    [HttpPut("{labelId}")]
    public async Task<ActionResult<ApiResponse<LabelDto>>> UpdateLabel(int labelId, [FromBody] string newName)
    {
        var updatedLabel = await labelService.UpdateLabelAsync(labelId, newName);
        var dto = mapper.Map<LabelDto>(updatedLabel);
        return Ok(ApiResponse<LabelDto>.ReturnSuccess(dto));
    }

    /// <summary>
    /// Delete a label.
    /// </summary>
    [HttpDelete("{labelId}")]
    public async Task<ActionResult<ApiResponse<string>>> DeleteLabel(int labelId)
    {
        await labelService.DeleteLabelAsync(labelId);
        return Ok(ApiResponse<string>.ReturnSuccess("Label deleted successfully."));
    }
}
