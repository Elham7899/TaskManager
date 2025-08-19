using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskManager.Domain.Entities;
using TaskManager.Infrastructure.DBContext;

namespace TaskManager.Application.Labels.Commands.Assign;

public class AssignLabelsToTaskCommandHandler(ApplicationDbContext dbContext) : IRequestHandler<AssignLabelsToTaskCommand>
{
    public async Task Handle(AssignLabelsToTaskCommand request, CancellationToken cancellationToken)
    {
        var task = await dbContext.Tasks
            .Include(x => x.TaskLabels)
            .ThenInclude(tl => tl.Label)
            .FirstOrDefaultAsync(x => x.Id == request.TaskId, cancellationToken);

        if (task is null)
            throw new KeyNotFoundException("Task not found.");

        var labels = await dbContext.Labels
            .Where(x => request.LabelIds.Contains(x.Id))
            .ToListAsync(cancellationToken);

        foreach (var label in labels)
        {
            // Check if already assigned
            if (!task.TaskLabels.Any(x => x.LabelId == label.Id))
            {
                task.TaskLabels.Add(new TaskLabel
                {
                    TaskId = task.Id,
                    LabelId = label.Id,
                    Task = task,
                    Label = label
                });
            }
            label.UpdatedBy = task.UpdatedBy;
            label.CreatedBy = task.CreatedBy;
        }

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}