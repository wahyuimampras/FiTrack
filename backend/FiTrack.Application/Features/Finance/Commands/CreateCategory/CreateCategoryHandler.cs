using MediatR;
using AutoMapper;
using FiTrack.Domain.Entities;
using FiTrack.Domain.Interfaces.Repositories;
using FiTrack.Application.Interfaces;
using FiTrack.Application.DTOs.Finance;

namespace FiTrack.Application.Features.Finance.Commands.CreateCategory;

public class CreateCategoryHandler(
    ICategoryRepository repo,
    ICurrentUserService currentUser,
    IMapper mapper
) : IRequestHandler<CreateCategoryCommand, CategoryDto>
{
    public async Task<CategoryDto> Handle(
        CreateCategoryCommand request,
        CancellationToken ct)
    {
        var category = Category.Create(
            currentUser.UserId,
            request.Name,
            request.Type,
            request.Icon,
            request.Color
        );

        await repo.AddAsync(category, ct);
        await repo.SaveChangesAsync(ct);

        return mapper.Map<CategoryDto>(category);
    }
}