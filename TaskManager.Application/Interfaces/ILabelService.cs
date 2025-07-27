using TaskManager.Application.DTOs.Label;
using TaskManager.Domain.Entities;

namespace TaskManager.Application.Interfaces;

public interface ILabelService
{
    Task<Label> CreateLabelAsync(CreateLabelDto input);
    Task<List<LabelDto>> GetAllLabelsAsync( int page, int pagesize);
    Task DeleteLabelAsync(int id);
    Task AssignLabelsToTaskAsync(int taskId, List<int> labels);
    Task RemoveLabelsFromTaskAsync(int taskId, int labelId);
    Task<Label> UpdateLabelAsync(int id, string name);
    Task<LabelDto> GetById(int id);
}
