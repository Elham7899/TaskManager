using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManager.API.DTOs;
using TaskManager.Application.DTOs.Label;
using TaskManager.Application.Interfaces;

namespace TaskManager.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LabelsController(ILabelService labelService, IMapper mapper) : ControllerBase
{
    private readonly ILabelService _labelService = labelService;
    private readonly IMapper _mapper = mapper;

    [HttpPost]
    public async Task<IActionResult> Create(CreateLabelDto input)
    {
        var label = await _labelService.CreateLabelAsync(input);
        var dto = _mapper.Map<LabelDto>(label);
        return Ok(dto);
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var labels = await _labelService.GetAllLabelsAsync();
        var dtos = _mapper.Map<List<LabelDto>>(labels);
        return Ok(dtos);
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
        var updatedLabel = await _labelService.UpdateLabelAsync(labelId, newName);
        var dto = _mapper.Map<LabelDto>(updatedLabel);
        return Ok(dto);
    }

    [Authorize]
    [HttpDelete("{labelId}")]
    public async Task<IActionResult> DeleteLabel(int labelId)
    {
        await labelService.DeleteLabelAsync(labelId);
        return Ok();
    }
}
