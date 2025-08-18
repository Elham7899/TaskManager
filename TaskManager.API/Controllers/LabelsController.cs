using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Application.DTOs.Common;
using TaskManager.Application.DTOs.Label;
using TaskManager.Application.Labels.Commands.Assign;
using TaskManager.Application.Labels.Commands.Create;
using TaskManager.Application.Labels.Commands.Delete;
using TaskManager.Application.Labels.Commands.Remove;
using TaskManager.Application.Labels.Commands.Update;
using TaskManager.Application.Labels.Queries.GetAll;

namespace TaskManager.API.Controllers;

/// <summary>
/// Manages task labels (create, list, update, delete, assign to tasks)
/// </summary>
[Route("api/[controller]")]
[Authorize(Roles = "User")]
[ApiController]
public class LabelsController(IMediator mediator, IMapper mapper) : ControllerBase
{
    /// <summary>
    /// Create a new label.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<LabelDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<LabelDto>>> Create([FromBody] CreateLabelDto input)
    {
        var label = await mediator.Send(new CreateLabelCommand(input, User.Identity!.Name!));
        return Ok(ApiResponse<LabelDto>.ReturnSuccess(label));
    }

    /// <summary>
    /// Get all labels with pagination.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<List<LabelDto>>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<List<LabelDto>>>> Get([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var result = await mediator.Send(new GetAllLabelsQuery(page, pageSize));
        return Ok(ApiResponse<IEnumerable<LabelDto>>.ReturnSuccess(result.Items, result.GetMetadata()));
    }

    /// <summary>
    /// Assign labels to a specific task.
    /// </summary>
    [HttpPost("{taskId}/assign")]
    public async Task<ActionResult<ApiResponse<string>>> AssignLabelToTask(int taskId, [FromBody] AssignLabelsDto input)
    {
        await mediator.Send(new AssignLabelsToTaskCommand(taskId, input.LabelIds));
        return Ok(ApiResponse<string>.ReturnSuccess("Labels assigned successfully."));
    }

    /// <summary>
    /// Remove a label from a task.
    /// </summary>
    [HttpDelete("{taskId}/remove/{labelId}")]
    public async Task<ActionResult<ApiResponse<string>>> RemoveLabelFromTask(int taskId, int labelId)
    {
        await mediator.Send(new RemoveLabelFromTaskCommand(taskId, labelId));
        return Ok(ApiResponse<string>.ReturnSuccess("Label removed successfully."));
    }

    /// <summary>
    /// Update a label's name.
    /// </summary>
    [HttpPut("{labelId}")]
    public async Task<ActionResult<ApiResponse<LabelDto>>> UpdateLabel(int labelId, [FromBody] string newName)
    {
        var updatedLabel = await mediator.Send(new UpdateLabelCommand(labelId, newName, User.Identity!.Name!));
        return Ok(ApiResponse<LabelDto>.ReturnSuccess(updatedLabel));
    }

    /// <summary>
    /// Delete a label.
    /// </summary>
    [HttpDelete("{labelId}")]
    public async Task<ActionResult<ApiResponse<string>>> DeleteLabel(int labelId)
    {
        await mediator.Send(new DeleteLabelCommand(labelId));
        return Ok(ApiResponse<string>.ReturnSuccess("Label deleted successfully."));
    }
}
