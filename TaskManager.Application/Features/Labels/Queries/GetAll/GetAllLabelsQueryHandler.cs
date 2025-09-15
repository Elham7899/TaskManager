using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskManager.Application.DTOs.Common;
using TaskManager.Application.DTOs.Label;
using TaskManager.Infrastructure.Persistence;

namespace TaskManager.Application.Features.Labels.Queries.GetAll;

public class GetAllLabelsQueryHandler(ApplicationDbContext dbContext, IMapper mapper) : IRequestHandler<GetAllLabelsQuery, PagedResult<LabelDto>>
{
    public async Task<PagedResult<LabelDto>> Handle(GetAllLabelsQuery request, CancellationToken cancellationToken)
    {
        var query = dbContext.Labels.AsNoTracking();

        var total = await query.CountAsync(cancellationToken);
        var items = await query
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ProjectTo<LabelDto>(mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return new PagedResult<LabelDto>(items, total, request.Page, request.PageSize);
    }
}