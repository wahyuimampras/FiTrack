using AutoMapper;
using FiTrack.Application.DTOs.Finance;
using FiTrack.Application.Interfaces;
using FiTrack.Domain.Exceptions;
using FiTrack.Domain.Interfaces.Repositories;
using MediatR;

namespace FiTrack.Application.Features.Finance.Queries.GetCategoryById;

public class GetCategoryByIdHandler(
    ICategoryRepository categoryRepository,
    ICurrentUserService currentUserService,
    IMapper mapper) : IRequestHandler<GetCategoryByIdQuery, CategoryDto>
{
    public async Task<CategoryDto> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        var category = await categoryRepository.GetByIdAsync(request.Id, cancellationToken);
        
        if (category == null || category.UserId != currentUserService.UserId)
            throw new DomainException("Category not found.");

        return mapper.Map<CategoryDto>(category);
    }
}
