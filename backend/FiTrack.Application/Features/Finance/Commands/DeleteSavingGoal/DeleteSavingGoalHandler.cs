using FiTrack.Application.Interfaces;
using FiTrack.Domain.Interfaces.Repositories;
using MediatR;

namespace FiTrack.Application.Features.Finance.Commands.DeleteSavingGoal;

public class DeleteSavingGoalHandler(
    ISavingGoalRepository savingGoalRepository,
    ICurrentUserService currentUserService) : IRequestHandler<DeleteSavingGoalCommand, Unit>
{
    public async Task<Unit> Handle(DeleteSavingGoalCommand request, CancellationToken cancellationToken)
    {
        var userId = currentUserService.UserId;
        if (userId == Guid.Empty)
        {
            throw new UnauthorizedAccessException("User is not authenticated.");
        }

        var savingGoal = await savingGoalRepository.GetByIdAsync(request.Id, userId, cancellationToken);
        if (savingGoal == null)
        {
            throw new KeyNotFoundException("Saving goal not found.");
        }

        await savingGoalRepository.DeleteAsync(savingGoal, cancellationToken);
        await savingGoalRepository.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}