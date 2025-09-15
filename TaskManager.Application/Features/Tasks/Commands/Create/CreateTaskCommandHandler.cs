using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskManager.Application.DTOs.Task;
using TaskManager.Domain.Entities;
using TaskManager.Infrastructure.Persistence;

namespace TaskManager.Application.Features.Tasks.Commands.Create;

// Handler
public class CreateTaskCommandHandler : IRequestHandler<CreateTaskCommand, TaskDto>
{
    private readonly ApplicationDbContext _db;
    private readonly IMapper _mapper;

    public CreateTaskCommandHandler(ApplicationDbContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }
    public async Task<TaskDto> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<TaskItem>(request.Dto);
        entity.CreatedAt = DateTime.UtcNow;
        entity.CreatedBy = request.CurrentUserId;

        // Attach labels by Ids
        if (request.Dto.LabelIds != null && request.Dto.LabelIds.Count > 0)
        {
            var labels = await _db.Labels.Where(l => request.Dto.LabelIds.Contains(l.Id)).ToListAsync(cancellationToken);
            foreach (var label in labels)
            {
                entity.TaskLabels.Add(new TaskLabel { Label = label, Task = entity });
            }
        }

        // Attach labels by Names (create missing)
        if (request.Dto.LabelNames != null && request.Dto.LabelNames.Count > 0)
        {
            var normalized = request.Dto.LabelNames.Select(n => n.Trim()).Where(n => !string.IsNullOrWhiteSpace(n)).ToList();
            var existing = await _db.Labels.Where(l => normalized.Contains(l.Name)).ToListAsync(cancellationToken);
            var existingNames = existing.Select(l => l.Name).ToHashSet();
            var missing = normalized.Where(n => !existingNames.Contains(n));

            foreach (var name in missing)
            {
                var newLabel = new Label { Name = name, CreatedAt = DateTime.UtcNow, CreatedBy = request.CurrentUserId };
                _db.Labels.Add(newLabel);
                entity.TaskLabels.Add(new TaskLabel { Label = newLabel, Task = entity });
            }

            foreach (var label in existing)
            {
                entity.TaskLabels.Add(new TaskLabel { Label = label, Task = entity });
            }
        }

        _db.Tasks.Add(entity);
        await _db.SaveChangesAsync(cancellationToken);

        // Reload with labels for mapping
        await _db.Entry(entity).Collection(e => e.TaskLabels).Query().Include(tl => tl.Label).LoadAsync(cancellationToken);
        return _mapper.Map<TaskDto>(entity);
    }
}
