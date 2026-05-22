using FiTrack.Domain.Entities;

namespace FiTrack.Domain.Interfaces.Repositories;

public interface IAccountRepository
{
    Task<IEnumerable<Account>> GetByUserIdAsync(Guid userId, CancellationToken ct = default);
    Task<Account?> GetByIdAsync(Guid id, Guid userId, CancellationToken ct = default);
    Task AddAsync(Account account, CancellationToken ct = default);
    Task UpdateAsync(Account account, CancellationToken ct = default);
    Task DeleteAsync(Account account, CancellationToken ct = default);
    Task SaveChangesAsync(CancellationToken ct = default);
}