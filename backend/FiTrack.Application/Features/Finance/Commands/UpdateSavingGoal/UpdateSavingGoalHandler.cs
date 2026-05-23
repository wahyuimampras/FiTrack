using FiTrack.Application.Interfaces;
using FiTrack.Domain.Interfaces.Repositories;
using MediatR;

namespace FiTrack.Application.Features.Finance.Commands.UpdateSavingGoal;

public class UpdateSavingGoalHandler(
    ISavingGoalRepository savingGoalRepository,
    ICurrentUserService currentUserService) : IRequestHandler<UpdateSavingGoalCommand, Unit>
{
    public async Task<Unit> Handle(UpdateSavingGoalCommand request, CancellationToken cancellationToken)
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

        savingGoal.Update(
            request.Name,
            request.TargetAmount,
            request.TargetDate);

        await savingGoalRepository.UpdateAsync(savingGoal, cancellationToken);
        await savingGoalRepository.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}