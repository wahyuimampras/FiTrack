using FiTrack.Application.Interfaces;
using FiTrack.Domain.Interfaces.Repositories;
using MediatR;

namespace FiTrack.Application.Features.Finance.Commands.DeleteBudget;

public class DeleteBudgetHandler(IBudgetRepository repo, ICurrentUserService user) : IRequestHandler<DeleteBudgetCommand, Unit> 
{
    public async Task<Unit> Handle(DeleteBudgetCommand req, CancellationToken ct) 
    {
        var budget = await repo.GetByIdAsync(req.Id, user.UserId, ct);
        if (budget == null) throw new KeyNotFoundException("Budget tidak ditemukan.");
        
        await repo.DeleteAsync(budget, ct);
        await repo.SaveChangesAsync(ct);
        return Unit.Value;
    }
}