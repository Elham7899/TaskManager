using MediatR;
using TaskManager.Application.DTOs.Label;

namespace TaskManager.Application.Labels.Commands.Create;

public record CreateLabelCommand(CreateLabelDto Input, string UserId) : IRequest<LabelDto>;
