using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskManager.Application.DTOs.Common;
using TaskManager.Application.DTOs.Task;
using TaskManager.Infrastructure.DBContext;

namespace TaskManager.Application.Tasks.Queries.GetAll;

public class GetTasksQueryHandler : IRequestHandler<GetTasksQuery, ApiResponse<PagedResult<TaskDto>>>
{
    private readonly ApplicationDbContext _db;
    private readonly IMapper _mapper;

    public GetTasksQueryHandler(ApplicationDbContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    public async Task<ApiResponse<PagedResult<TaskDto>>> Handle(GetTasksQuery request, CancellationToken cancellationToken)
    {
        var query = _db.Tasks.AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.Search))
            query = query.Where(t => t.Title.ToLower().Contains(request.Search.ToLower()));

        if (request.IsCompleted.HasValue)
            query = query.Where(t => t.IsCompleted == request.IsCompleted);

        if (request.LabelId.HasValue)
            query = query.Where(t => t.TaskLabels.Any(tl => tl.LabelId == request.LabelId));

        var total = await query.CountAsync(cancellationToken);

        var items = await query
            .Include(t => t.TaskLabels).ThenInclude(tl => tl.Label)
            .OrderByDescending(t => t.CreatedAt)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ProjectTo<TaskDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        var result = new PagedResult<TaskDto>(items, total, request.Page, request.PageSize);

        return ApiResponse<PagedResult<TaskDto>>.ReturnSuccess(result);
    }
}
