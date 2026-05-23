using FiTrack.Application.Interfaces;
using FiTrack.Domain.Interfaces.Repositories;
using MediatR;

namespace FiTrack.Application.Features.Finance.Commands.UpdateBudget;

public class UpdateBudgetHandler(IBudgetRepository repo, ICurrentUserService user) : IRequestHandler<UpdateBudgetCommand, Unit> 
{
    public async Task<Unit> Handle(UpdateBudgetCommand req, CancellationToken ct) 
    {
        var budget = await repo.GetByIdAsync(req.Id, user.UserId, ct);
        if (budget == null) throw new KeyNotFoundException("Budget tidak ditemukan.");
        
        // Pastikan Anda sudah menambahkan method Update di class Budget.cs
        budget.Update(req.CategoryId, req.Amount, req.Month, req.Year);
        
        await repo.UpdateAsync(budget, ct);
        await repo.SaveChangesAsync(ct);
        return Unit.Value;
    }
}