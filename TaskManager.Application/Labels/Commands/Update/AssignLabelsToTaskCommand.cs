using MediatR;

namespace TaskManager.Application.Labels.Commands.Assign;

public record AssignLabelsToTaskCommand(int TaskId, List<int> LabelIds) : IRequest;
