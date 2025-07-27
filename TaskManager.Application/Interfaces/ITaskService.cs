using TaskManager.Application.DTOs.Task;
using TaskManager.Domain.Entities;

namespace TaskManager.Application.Interfaces;

public interface ITaskService
{
    Task<TaskItem> AddTaskAsync(TaskItem task, List<int>? labelIds = null);
    Task<TaskDto?> GetTaskByIdAsync(int id);
    Task<List<TaskDto>> GetAllTasksAsync(bool isComplete, int page, int pagesize);
}