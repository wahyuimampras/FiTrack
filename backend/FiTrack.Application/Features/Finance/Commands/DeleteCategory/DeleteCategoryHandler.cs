using MediatR;
using FiTrack.Domain.Interfaces.Repositories;
using FiTrack.Application.Interfaces;

namespace FiTrack.Application.Features.Finance.Commands.DeleteCategory;

public class DeleteCategoryHandler(
    ICategoryRepository repo,
    ICurrentUserService currentUser
) : IRequestHandler<DeleteCategoryCommand, Unit>
{
    public async Task<Unit> Handle(
        DeleteCategoryCommand request,
        CancellationToken ct)
    {
        var category = await repo.GetByIdAsync(request.Id, ct);

        if (category == null || category.UserId != currentUser.UserId)
        {
            throw new Exception("Category not found or access denied.");
        }
        
        if (category.IsDefault)
        {
            throw new Exception("Default categories cannot be deleted.");
        }

        repo.Delete(category);
        await repo.SaveChangesAsync(ct);

        return Unit.Value;
    }
}