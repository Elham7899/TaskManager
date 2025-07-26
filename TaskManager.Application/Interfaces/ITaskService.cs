using TaskManager.Domain.Entities;

namespace TaskManager.Application.Interfaces;

public interface ITaskService
{
    Task<TaskItem> AddTaskAsync(TaskItem task, List<int>? labelIds = null);
    Task<TaskItem?> GetTaskByIdAsync(int id);
    Task<List<TaskItem>> GetAllTasksAsync();
}