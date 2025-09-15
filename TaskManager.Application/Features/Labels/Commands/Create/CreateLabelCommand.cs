using MediatR;
using TaskManager.Application.DTOs.Label;

namespace TaskManager.Application.Features.Labels.Commands.Create;

public record CreateLabelCommand(CreateLabelDto Input, string UserId) : IRequest<LabelDto>;
