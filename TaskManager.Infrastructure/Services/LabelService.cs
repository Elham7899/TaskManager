using Microsoft.EntityFrameworkCore;
using TaskManager.Application.DTOs;
using TaskManager.Application.Interfaces;
using TaskManager.Domain.Entities;
using TaskManager.Infrastructure.DBContext;

namespace TaskManager.Infrastructure.Services;

public class LabelService(ApplicationDbContext context) : ILabelService
{
    public async Task AssignLabelsToTaskAsync(int taskId, List<int> labelIds)
    {
        var task = await context.Tasks.FirstOrDefaultAsync(x => x.Id == taskId);
        if (task == null)
            throw new Exception("Task not found");

        var labels = await context.Labels.Where(x => labelIds.Contains(x.Id)).ToListAsync();
        task.Labels = labels;

        await context.SaveChangesAsync();
    }

    public async Task<Label> CreateLabelAsync(CreateLabelDto input)
    {
        var label = new Label { Name = input.Name };
        context.Labels.Add(label);
        await context.SaveChangesAsync();
        return label;
    }

    public async Task DeleteLabelAsync(int id)
    {
        var label = await context.Labels.Where(x => x.Id == id).FirstOrDefaultAsync();
        context.Labels.Remove(label);
        await context.SaveChangesAsync();
    }

    public async Task<List<Label>> GetAllLabelsAsync()
    {
        return await context.Labels.ToListAsync();
    }

    public async Task<Label> GetById(int id)
    {
        return await context.Labels.Where(x => x.Id == id).SingleOrDefaultAsync();
    }

    public async Task RemoveLabelsFromTaskAsync(int taskId, int labelId)
    {
        var task = await context.Tasks.FirstOrDefaultAsync(x => x.Id == taskId);
        if (task == null)
            throw new Exception("Task not found");

        var label = await context.Labels.Where(x => x.Id == labelId).FirstOrDefaultAsync();
        if (label != null)
        {
            task.Labels.Remove(label);
            await context.SaveChangesAsync();
        }
    }

    public async Task<Label> UpdateLabelAsync(int id, string name)
    {
        var label = await context.Labels.Where(x => x.Id == id).FirstOrDefaultAsync();

        if (label is null)
            throw new Exception("Label not found");

        label.Name = name;

        await context.SaveChangesAsync();
        return label;
    }
}
