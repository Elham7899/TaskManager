using MediatR;

namespace TaskManager.Application.Labels.Commands.Delete;

public record DeleteLabelCommand(int labelId,string userName) : IRequest;
