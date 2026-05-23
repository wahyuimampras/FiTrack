using FiTrack.Domain.Entities;

namespace FiTrack.Domain.Interfaces.Repositories;

public interface ISavingGoalRepository
{
    Task<IEnumerable<SavingGoal>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<SavingGoal?> GetByIdAsync(Guid id, Guid userId, CancellationToken cancellationToken = default);
    Task AddAsync(SavingGoal savingGoal, CancellationToken cancellationToken = default);
    Task UpdateAsync(SavingGoal savingGoal, CancellationToken cancellationToken = default);
    Task DeleteAsync(SavingGoal savingGoal, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}