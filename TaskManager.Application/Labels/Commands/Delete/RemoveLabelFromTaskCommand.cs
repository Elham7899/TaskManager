using MediatR;

namespace TaskManager.Application.Labels.Commands.Remove;

public record RemoveLabelFromTaskCommand(int TaskId, int LabelId) : IRequest;
