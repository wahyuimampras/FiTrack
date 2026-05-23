using FiTrack.Application.Interfaces;
using FiTrack.Domain.Entities;
using FiTrack.Domain.Interfaces.Repositories;
using MediatR;

namespace FiTrack.Application.Features.Finance.Commands.CreateSavingGoal;

public class CreateSavingGoalHandler(
    ISavingGoalRepository savingGoalRepository,
    ICurrentUserService currentUserService) : IRequestHandler<CreateSavingGoalCommand, Guid>
{
    public async Task<Guid> Handle(CreateSavingGoalCommand request, CancellationToken cancellationToken)
    {
        var userId = currentUserService.UserId;
        if (userId == Guid.Empty)
        {
            throw new UnauthorizedAccessException("User is not authenticated.");
        }

        var savingGoal = SavingGoal.Create(
            userId,
            request.Name,
            request.TargetAmount,
            request.TargetDate);

        await savingGoalRepository.AddAsync(savingGoal, cancellationToken);
        await savingGoalRepository.SaveChangesAsync(cancellationToken);

        return savingGoal.Id;
    }
}