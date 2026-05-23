using FiTrack.Domain.Entities;

namespace FiTrack.Domain.Interfaces.Repositories;

public interface IBudgetRepository
{
    Task<IEnumerable<Budget>> GetByUserIdAsync(Guid userId, short? month, short? year, CancellationToken ct = default);
    Task<Budget?> GetByIdAsync(Guid id, Guid userId, CancellationToken ct = default);
    Task<Budget?> GetByCategoryAndPeriodAsync(Guid userId, Guid categoryId, short month, short year, CancellationToken ct = default);
    Task AddAsync(Budget budget, CancellationToken ct = default);
    Task UpdateAsync(Budget budget, CancellationToken ct = default);
    Task DeleteAsync(Budget budget, CancellationToken ct = default);
    Task SaveChangesAsync(CancellationToken ct = default);
}