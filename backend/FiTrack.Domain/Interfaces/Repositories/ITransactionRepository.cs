using FiTrack.Domain.Entities;

namespace FiTrack.Domain.Interfaces.Repositories;

public interface ITransactionRepository
{
    Task<Transaction?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<IEnumerable<Transaction>> GetUserTransactionsAsync(Guid userId, CancellationToken ct = default);
    Task AddAsync(Transaction transaction, CancellationToken ct = default);
    void Update(Transaction transaction);
    void Delete(Transaction transaction);
    Task SaveChangesAsync(CancellationToken ct = default);
    Task<List<Transaction>> GetByDateRangeAsync(Guid userId, DateTime startDate, DateTime endDate, CancellationToken ct = default);
}