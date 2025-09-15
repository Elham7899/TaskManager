using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskManager.Infrastructure.Persistence;

namespace TaskManager.Application.Features.Labels.Commands.Delete;

public class RemoveLabelFromTaskCommandHandler(ApplicationDbContext dbContext) : IRequestHandler<RemoveLabelFromTaskCommand>
{
    public async Task Handle(RemoveLabelFromTaskCommand request, CancellationToken cancellationToken)
    {
        var task = await dbContext.Tasks
            .Include(x => x.TaskLabels)
            .FirstOrDefaultAsync(x => x.Id == request.TaskId, cancellationToken);

        if (task is null)
            throw new KeyNotFoundException("Task not found");

        var taskLabels = task.TaskLabels.FirstOrDefault(x => x.LabelId == request.LabelId);
        if (taskLabels is null)
            throw new KeyNotFoundException("Label is not assigned to this task");

        //Removing relations
        task.TaskLabels.Remove(taskLabels);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
