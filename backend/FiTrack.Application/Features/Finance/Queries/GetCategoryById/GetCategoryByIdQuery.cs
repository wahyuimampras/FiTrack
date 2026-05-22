using FiTrack.Application.DTOs.Finance;
using MediatR;

namespace FiTrack.Application.Features.Finance.Queries.GetCategoryById;

public record GetCategoryByIdQuery(Guid Id) : IRequest<CategoryDto>;
