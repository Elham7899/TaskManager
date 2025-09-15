using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskManager.Application.DTOs.Label;
using TaskManager.Infrastructure.Persistence;

namespace TaskManager.Application.Features.Labels.Commands.Update;

public class UpdateLabelCommandHandler(ApplicationDbContext dbContext, IMapper mapper) : IRequestHandler<UpdateLabelCommand, LabelDto>
{
    public async Task<LabelDto> Handle(UpdateLabelCommand request, CancellationToken cancellationToken)
    {
        var label = await dbContext.Labels.FirstOrDefaultAsync(x => x.Id == request.labelId, cancellationToken);
       
        if (label == null)
            throw new KeyNotFoundException("Label not found");

        label.Name = request.name;
        label.UpdatedBy = request.userName;
        label.UpdatedAt = DateTime.UtcNow;

        await dbContext.SaveChangesAsync(cancellationToken);
        return mapper.Map<LabelDto>(label);
    }
}
