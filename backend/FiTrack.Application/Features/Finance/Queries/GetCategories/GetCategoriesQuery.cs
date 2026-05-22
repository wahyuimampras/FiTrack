using MediatR;
using FiTrack.Application.DTOs.Finance;

namespace FiTrack.Application.Features.Finance.Queries.GetCategories;

public record GetCategoriesQuery() : IRequest<IEnumerable<CategoryDto>>;