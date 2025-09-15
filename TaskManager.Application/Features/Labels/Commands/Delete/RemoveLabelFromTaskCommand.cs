using MediatR;

namespace TaskManager.Application.Features.Labels.Commands.Delete;

public record RemoveLabelFromTaskCommand(int TaskId, int LabelId) : IRequest;
