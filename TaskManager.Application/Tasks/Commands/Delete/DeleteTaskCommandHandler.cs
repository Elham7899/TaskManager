using MediatR;
using Microsoft.EntityFrameworkCore;
using TaskManager.Infrastructure.DBContext;

namespace TaskManager.Application.Tasks.Commands.Delete;

public class DeleteTaskCommandHandler : IRequestHandler<DeleteTaskCommand, Unit>
{
    private readonly ApplicationDbContext _db;
    public DeleteTaskCommandHandler(ApplicationDbContext db) { _db = db; }

    public async Task<Unit> Handle(DeleteTaskCommand request, CancellationToken ct)
    {
        var entity = await _db.Tasks.FirstOrDefaultAsync(t => t.Id == request.Id, ct);
        if (entity == null) return Unit.Value; // idempotent

        _db.Tasks.Remove(entity);
        await _db.SaveChangesAsync(ct);
        return Unit.Value;
    }
}