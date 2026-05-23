using FiTrack.Domain.Entities;
using FiTrack.Domain.Interfaces.Repositories;
using FiTrack.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FiTrack.Infrastructure.Persistence.Repositories;

public class RecurringBillRepository(AppDbContext dbContext) : IRecurringBillRepository
{
    public async Task<IEnumerable<RecurringBill>> GetByUserIdAsync(Guid userId, CancellationToken ct = default)
    {
        return await dbContext.RecurringBills
            .Include(b => b.Category)
            .Where(b => b.UserId == userId)
            .OrderBy(b => b.DueDay)
            .ToListAsync(ct);
    }

    public async Task<RecurringBill?> GetByIdAsync(Guid id, Guid userId, CancellationToken ct = default)
    {
        return await dbContext.RecurringBills
            .Include(b => b.Category)
            .FirstOrDefaultAsync(b => b.Id == id && b.UserId == userId, ct);
    }

    public async Task AddAsync(RecurringBill bill, CancellationToken ct = default) => await dbContext.RecurringBills.AddAsync(bill, ct);
    public Task UpdateAsync(RecurringBill bill, CancellationToken ct = default) { dbContext.RecurringBills.Update(bill); return Task.CompletedTask; }
    public Task DeleteAsync(RecurringBill bill, CancellationToken ct = default) { dbContext.RecurringBills.Remove(bill); return Task.CompletedTask; }
    public async Task SaveChangesAsync(CancellationToken ct = default) => await dbContext.SaveChangesAsync(ct);
}