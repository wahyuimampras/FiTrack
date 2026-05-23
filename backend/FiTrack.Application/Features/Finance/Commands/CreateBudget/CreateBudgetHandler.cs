using FiTrack.Application.Interfaces;
using FiTrack.Domain.Entities;
using FiTrack.Domain.Interfaces.Repositories;
using MediatR;

namespace FiTrack.Application.Features.Finance.Commands.CreateBudget;

public class CreateBudgetHandler(
    IBudgetRepository budgetRepository,
    ICurrentUserService currentUserService) : IRequestHandler<CreateBudgetCommand, Guid>
{
    public async Task<Guid> Handle(CreateBudgetCommand request, CancellationToken cancellationToken)
    {
        var userId = currentUserService.UserId;
        if (userId == Guid.Empty)
        {
            throw new UnauthorizedAccessException("User is not authenticated.");
        }

        // Logic pembuatan budget
        var budget = Budget.Create(
            userId,
            request.CategoryId,
            request.Amount,
            request.Month,
            request.Year);

        await budgetRepository.AddAsync(budget, cancellationToken);
        await budgetRepository.SaveChangesAsync(cancellationToken);

        return budget.Id;
    }
}