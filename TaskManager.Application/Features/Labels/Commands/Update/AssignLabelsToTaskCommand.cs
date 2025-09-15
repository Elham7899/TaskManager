using MediatR;

namespace TaskManager.Application.Features.Labels.Commands.Update;

public record AssignLabelsToTaskCommand(int TaskId, List<int> LabelIds) : IRequest;
