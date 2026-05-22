using MediatR;
using AutoMapper;
using FiTrack.Domain.Interfaces.Repositories;
using FiTrack.Application.Interfaces;
using FiTrack.Application.DTOs.Finance;

namespace FiTrack.Application.Features.Finance.Commands.UpdateCategory;

public class UpdateCategoryHandler(
    ICategoryRepository repo,
    ICurrentUserService currentUser,
    IMapper mapper
) : IRequestHandler<UpdateCategoryCommand, CategoryDto>
{
    public async Task<CategoryDto> Handle(
        UpdateCategoryCommand request,
        CancellationToken ct)
    {
        var category = await repo.GetByIdAsync(request.Id, ct);

        if (category == null || (category.UserId != currentUser.UserId && !category.IsDefault))
        {
            throw new Exception("Category not found or access denied.");
        }

        if (category.IsDefault)
        {
             throw new Exception("Default categories cannot be updated.");
        }

        category.Update(request.Name, request.Icon, request.Color);

        repo.Update(category);
        await repo.SaveChangesAsync(ct);

        return mapper.Map<CategoryDto>(category);
    }
}