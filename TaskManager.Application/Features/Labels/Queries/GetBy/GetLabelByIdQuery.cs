using MediatR;
using TaskManager.Application.DTOs.Label;

namespace TaskManager.Application.Features.Labels.Queries.GetBy;

public record GetLabelByIdQuery(int id) : IRequest<LabelDto?>;
