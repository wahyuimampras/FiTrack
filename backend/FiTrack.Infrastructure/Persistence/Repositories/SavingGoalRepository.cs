using FiTrack.Domain.Entities;
using FiTrack.Domain.Interfaces.Repositories;
using FiTrack.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FiTrack.Infrastructure.Persistence.Repositories;

public class SavingGoalRepository(AppDbContext dbContext) : ISavingGoalRepository
{
    public async Task<IEnumerable<SavingGoal>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await dbContext.SavingGoals
            .Where(s => s.UserId == userId)
            .OrderByDescending(s => s.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<SavingGoal?> GetByIdAsync(Guid id, Guid userId, CancellationToken cancellationToken = default)
    {
        return await dbContext.SavingGoals
            .FirstOrDefaultAsync(s => s.Id == id && s.UserId == userId, cancellationToken);
    }

    public async Task AddAsync(SavingGoal savingGoal, CancellationToken cancellationToken = default)
    {
        await dbContext.SavingGoals.AddAsync(savingGoal, cancellationToken);
    }

    public Task UpdateAsync(SavingGoal savingGoal, CancellationToken cancellationToken = default)
    {
        dbContext.SavingGoals.Update(savingGoal);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(SavingGoal savingGoal, CancellationToken cancellationToken = default)
    {
        dbContext.SavingGoals.Remove(savingGoal);
        return Task.CompletedTask;
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}