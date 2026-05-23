using FiTrack.Domain.Entities;

namespace FiTrack.Domain.Interfaces.Repositories;

public interface IRecurringBillRepository
{
    Task<IEnumerable<RecurringBill>> GetByUserIdAsync(Guid userId, CancellationToken ct = default);
    Task<RecurringBill?> GetByIdAsync(Guid id, Guid userId, CancellationToken ct = default);
    Task AddAsync(RecurringBill bill, CancellationToken ct = default);
    Task UpdateAsync(RecurringBill bill, CancellationToken ct = default);
    Task DeleteAsync(RecurringBill bill, CancellationToken ct = default);
    Task SaveChangesAsync(CancellationToken ct = default);
}