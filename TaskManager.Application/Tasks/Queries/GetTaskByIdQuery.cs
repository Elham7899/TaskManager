using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskManager.Application.DTOs.Task;
using TaskManager.Infrastructure.DBContext;

namespace TaskManager.Application.Tasks.Queries;

public record GetTaskByIdQuery(int Id) : IRequest<TaskDto?>;

public class GetTaskByIdQueryHandler : IRequestHandler<GetTaskByIdQuery, TaskDto?>
{
    private readonly ApplicationDbContext _db;
    private readonly IMapper _mapper;
    public GetTaskByIdQueryHandler(ApplicationDbContext db, IMapper mapper)
    {
        _db = db; _mapper = mapper;
    }

    public async Task<TaskDto?> Handle(GetTaskByIdQuery request, CancellationToken cancellationToken)
    {
        var task = await _db.Tasks
            .Include(t => t.TaskLabels).ThenInclude(tl => tl.Label)
            .FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken);

        return task == null ? null : _mapper.Map<TaskDto>(task);
    }
}