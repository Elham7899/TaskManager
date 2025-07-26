using Microsoft.EntityFrameworkCore;
using TaskManager.Application.Interfaces;
using TaskManager.Domain.Entities;
using TaskManager.Infrastructure.DBContext;

namespace TaskManager.Infrastructure.Services;

public class TaskService : ITaskService
{
    private readonly ApplicationDbContext _context;

    public TaskService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<TaskItem> AddTaskAsync(TaskItem task, List<int>? labelIds = null)
    {
        // Clear existing labels to avoid duplicates
        task.Labels.Clear();

        if (labelIds != null && labelIds.Any())
        {
            var labels = await _context.Labels.Where(x => labelIds.Contains(x.Id)).ToListAsync();
            task.Labels.AddRange(labels);
        }

        _context.Add(task);
        await _context.SaveChangesAsync();
        return task;
    }

    public async Task<List<TaskItem>> GetAllTasksAsync() => await _context.Tasks.ToListAsync();

    public async Task<TaskItem?> GetTaskByIdAsync(int id) => await _context.Tasks.Include(t => t.Labels).FirstOrDefaultAsync(t => t.Id == id);
}
