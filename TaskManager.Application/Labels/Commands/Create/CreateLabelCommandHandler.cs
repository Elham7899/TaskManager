using AutoMapper;
using MediatR;
using TaskManager.Application.DTOs.Label;
using TaskManager.Domain.Entities;
using TaskManager.Infrastructure.DBContext;

namespace TaskManager.Application.Labels.Commands.Create;

public class CreateLabelCommandHandler(ApplicationDbContext dbContext, IMapper mapper) : IRequestHandler<CreateLabelCommand, LabelDto>
{
    public async Task<LabelDto> Handle(CreateLabelCommand request, CancellationToken cancellationToken)
    {
        var entity = mapper.Map<Label>(request.Input);
        entity.CreatedBy = request.UserId;
        entity.UpdatedBy = request.UserId;

        dbContext.Labels.Add(entity);
        await dbContext.SaveChangesAsync(cancellationToken);

        return mapper.Map<LabelDto>(entity);
    }
}