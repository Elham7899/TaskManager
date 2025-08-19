using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskManager.Application.DTOs.Label;
using TaskManager.Infrastructure.DBContext;

namespace TaskManager.Application.Labels.Queries.GetBy;

public class GetLabelByIdQueryHandler(ApplicationDbContext dbContext, IMapper mapper) : IRequestHandler<GetLabelByIdQuery, LabelDto>
{
    public async Task<LabelDto> Handle(GetLabelByIdQuery request, CancellationToken cancellationToken)
    {
        var label = await dbContext.Labels.FirstOrDefaultAsync(x => x.Id == request.id);

        return label == null ? null : mapper.Map<LabelDto>(label);
    }
}