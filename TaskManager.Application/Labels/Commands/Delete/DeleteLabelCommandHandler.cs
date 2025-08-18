using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskManager.Infrastructure.DBContext;

namespace TaskManager.Application.Labels.Commands.Delete;

public class DeleteLabelCommandHandler(ApplicationDbContext dbContext) : IRequestHandler<DeleteLabelCommand>
{
    public async Task Handle(DeleteLabelCommand request, CancellationToken cancellationToken)
    {
        var label = await dbContext.Labels.FirstOrDefaultAsync(x => x.Id == request.labelId, cancellationToken);
        if (label != null)
        {
            dbContext.Labels.Remove(label);
            await dbContext.SaveChangesAsync();
        }
        else throw new KeyNotFoundException("Label not found");
    }
}
