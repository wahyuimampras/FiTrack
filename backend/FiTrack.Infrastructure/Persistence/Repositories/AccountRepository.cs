using FiTrack.Domain.Entities;
using FiTrack.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FiTrack.Infrastructure.Persistence.Repositories;

public class AccountRepository(AppDbContext dbContext) : IAccountRepository
{
    public async Task<IEnumerable<Account>> GetByUserIdAsync(Guid userId, CancellationToken ct = default)
    {
        return await dbContext.Accounts
            .Where(a => a.UserId == userId && a.IsActive)
            .OrderByDescending(a => a.CreatedAt)
            .ToListAsync(ct);
    }

    public async Task<Account?> GetByIdAsync(Guid id, Guid userId, CancellationToken ct = default)
    {
        return await dbContext.Accounts
            .FirstOrDefaultAsync(a => a.Id == id && a.UserId == userId, ct);
    }

    public async Task AddAsync(Account account, CancellationToken ct = default)
    {
        await dbContext.Accounts.AddAsync(account, ct);
    }

    public Task UpdateAsync(Account account, CancellationToken ct = default)
    {
        dbContext.Accounts.Update(account);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Account account, CancellationToken ct = default)
    {
        dbContext.Accounts.Remove(account);
        return Task.CompletedTask;
    }

    public async Task SaveChangesAsync(CancellationToken ct = default)
    {
        await dbContext.SaveChangesAsync(ct);
    }
}