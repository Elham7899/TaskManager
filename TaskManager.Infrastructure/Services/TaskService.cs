using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using TaskManager.Application.DTOs.Task;
using TaskManager.Application.Interfaces;
using TaskManager.Domain.Entities;
using TaskManager.Infrastructure.DBContext;

namespace TaskManager.Infrastructure.Services;

public class TaskService : ITaskService
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;

    public TaskService(ApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
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

    public async Task<List<TaskDto>> GetAllTasksAsync(bool isComplete, int page = 1, int pageSize = 10)
    {
        var query = _context.Tasks.Include(x => x.Labels).AsQueryable();

        if (isComplete)
        {
            query = query.Where(x => x.IsCompleted);
        }

        return await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ProjectTo<TaskDto>(_mapper.ConfigurationProvider)
            .ToListAsync();
    }

    public async Task<TaskDto?> GetTaskByIdAsync(int id) =>
        await _context.Tasks
        .Include(t => t.Labels)
        .ProjectTo<TaskDto>(_mapper.ConfigurationProvider)
        .FirstOrDefaultAsync(t => t.Id == id);
}
