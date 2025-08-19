using MediatR;
using TaskManager.Application.DTOs.Label;

namespace TaskManager.Application.Labels.Commands.Update;

public record UpdateLabelCommand(int labelId, string name, string userName) : IRequest<LabelDto>;
