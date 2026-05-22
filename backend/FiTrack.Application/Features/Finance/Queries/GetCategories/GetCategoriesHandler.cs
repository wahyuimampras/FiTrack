using MediatR;
using AutoMapper;
using FiTrack.Domain.Interfaces.Repositories;
using FiTrack.Application.Interfaces;
using FiTrack.Application.DTOs.Finance;

namespace FiTrack.Application.Features.Finance.Queries.GetCategories;

public class GetCategoriesHandler(
    ICategoryRepository repo,
    ICurrentUserService currentUser,
    IMapper mapper
) : IRequestHandler<GetCategoriesQuery, IEnumerable<CategoryDto>>
{
    public async Task<IEnumerable<CategoryDto>> Handle(GetCategoriesQuery request, CancellationToken ct)
    {
        var categories = await repo.GetUserCategoriesAsync(currentUser.UserId, ct);
        return mapper.Map<IEnumerable<CategoryDto>>(categories);
    }
}