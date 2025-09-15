using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskManager.Application.DTOs.Task;
using TaskManager.Domain.Entities;
using TaskManager.Infrastructure.Persistence;

namespace TaskManager.Application.Features.Tasks.Commands.Update;

public class UpdateTaskCommandHandler : IRequestHandler<UpdateTaskCommand, TaskDto>
{
    private readonly ApplicationDbContext _db;
    private readonly IMapper _mapper;

    public UpdateTaskCommandHandler(ApplicationDbContext db, IMapper mapper)
    {
        _db = db;
        _mapper = mapper;
    }

    public async Task<TaskDto> Handle(UpdateTaskCommand request, CancellationToken ct)
    {
        var entity = await _db.Tasks
            .Include(t => t.TaskLabels).ThenInclude(tl => tl.Label)
            .FirstOrDefaultAsync(t => t.Id == request.Dto.Id, ct);

        if (entity == null)
            throw new KeyNotFoundException($"Task {request.Dto.Id} not found");

        entity.Title = request.Dto.Title;
        entity.IsCompleted = request.Dto.IsCompleted;
        entity.UpdatedAt = DateTime.UtcNow;
        entity.UpdatedBy = request.CurrentUserId;

        // Sync labels if provided
        if (request.Dto.LabelIds != null && request.Dto.LabelIds.Any() ||
            request.Dto.LabelNames != null && request.Dto.LabelNames.Any())
        {
            var desiredLabelIds = new HashSet<int>();

            if (request.Dto.LabelIds != null)
            {
                foreach (var id in request.Dto.LabelIds)
                    desiredLabelIds.Add(id);
            }

            if (request.Dto.LabelNames != null)
            {
                var names = request.Dto.LabelNames.Select(n => n.Trim()).Where(n => !string.IsNullOrWhiteSpace(n)).ToList();
                var existing = await _db.Labels.Where(l => names.Contains(l.Name)).ToListAsync(ct);
                var existingNames = existing.Select(l => l.Name).ToHashSet();
                var missing = names.Where(n => !existingNames.Contains(n));

                foreach (var name in missing)
                {
                    var newLabel = new Label { Name = name, CreatedAt = DateTime.UtcNow, CreatedBy = request.CurrentUserId };
                    _db.Labels.Add(newLabel);
                    await _db.SaveChangesAsync(ct); // ensure Id
                    desiredLabelIds.Add(newLabel.Id);
                }

                foreach (var l in existing)
                    desiredLabelIds.Add(l.Id);
            }

            // current ids
            var currentIds = entity.TaskLabels.Select(tl => tl.LabelId).ToHashSet();

            // remove
            var toRemove = entity.TaskLabels.Where(tl => !desiredLabelIds.Contains(tl.LabelId)).ToList();
            foreach (var rem in toRemove)
                entity.TaskLabels.Remove(rem);

            // add
            var toAddIds = desiredLabelIds.Except(currentIds).ToList();
            if (toAddIds.Count > 0)
            {
                var labelsToAdd = await _db.Labels.Where(l => toAddIds.Contains(l.Id)).ToListAsync(ct);
                foreach (var label in labelsToAdd)
                    entity.TaskLabels.Add(new TaskLabel { TaskId = entity.Id, LabelId = label.Id });
            }
        }

        await _db.SaveChangesAsync(ct);
        return _mapper.Map<TaskDto>(entity);
    }
}
