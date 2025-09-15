using MediatR;
using TaskManager.Application.DTOs.Common;
using TaskManager.Application.DTOs.Label;

namespace TaskManager.Application.Features.Labels.Queries.GetAll;

public record GetAllLabelsQuery(int Page, int PageSize) : IRequest<PagedResult<LabelDto>>;
