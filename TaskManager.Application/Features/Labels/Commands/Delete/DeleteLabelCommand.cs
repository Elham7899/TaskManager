using MediatR;

namespace TaskManager.Application.Features.Labels.Commands.Delete;

public record DeleteLabelCommand(int labelId,string userName) : IRequest;
