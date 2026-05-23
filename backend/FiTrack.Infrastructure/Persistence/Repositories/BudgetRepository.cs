using FiTrack.Domain.Entities;
using FiTrack.Domain.Interfaces.Repositories;
using FiTrack.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FiTrack.Infrastructure.Persistence.Repositories;

public class BudgetRepository(AppDbContext dbContext) : IBudgetRepository
{
    public async Task<IEnumerable<Budget>> GetByUserIdAsync(Guid userId, short? month, short? year, CancellationToken ct = default)
    {
        var query = dbContext.Budgets.Include(b => b.Category).Where(b => b.UserId == userId);
        if (month.HasValue) query = query.Where(b => b.Month == month.Value);
        if (year.HasValue) query = query.Where(b => b.Year == year.Value);
        return await query.OrderByDescending(b => b.Year).ThenByDescending(b => b.Month).ToListAsync(ct);
    }

    public async Task<Budget?> GetByIdAsync(Guid id, Guid userId, CancellationToken ct = default)
        => await dbContext.Budgets.Include(b => b.Category).FirstOrDefaultAsync(b => b.Id == id && b.UserId == userId, ct);

    public async Task<Budget?> GetByCategoryAndPeriodAsync(Guid userId, Guid categoryId, short month, short year, CancellationToken ct = default)
        => await dbContext.Budgets.FirstOrDefaultAsync(b => b.UserId == userId && b.CategoryId == categoryId && b.Month == month && b.Year == year, ct);

    public async Task AddAsync(Budget budget, CancellationToken ct = default) => await dbContext.Budgets.AddAsync(budget, ct);
    public Task UpdateAsync(Budget budget, CancellationToken ct = default) { dbContext.Budgets.Update(budget); return Task.CompletedTask; }
    public Task DeleteAsync(Budget budget, CancellationToken ct = default) { dbContext.Budgets.Remove(budget); return Task.CompletedTask; }
    public async Task SaveChangesAsync(CancellationToken ct = default) => await dbContext.SaveChangesAsync(ct);
}