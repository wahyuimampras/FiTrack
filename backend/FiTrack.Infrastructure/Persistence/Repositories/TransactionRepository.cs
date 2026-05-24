using FiTrack.Domain.Entities;
using FiTrack.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FiTrack.Infrastructure.Persistence.Repositories;

public class TransactionRepository(AppDbContext dbContext) : ITransactionRepository
{
    public async Task<Transaction?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await dbContext.Transactions
            .Include(t => t.Category)
            .Include(t => t.Account)
            .FirstOrDefaultAsync(t => t.Id == id, ct);
    }

    public async Task<IEnumerable<Transaction>> GetUserTransactionsAsync(Guid userId, CancellationToken ct = default)
    {
        return await dbContext.Transactions
            .Include(t => t.Category)
            .Include(t => t.Account)
            .Where(t => t.UserId == userId)
            .OrderByDescending(t => t.Date)
            .ToListAsync(ct);
    }

    public async Task<(IEnumerable<Transaction> Items, int TotalCount)> GetPagedUserTransactionsAsync(
        Guid userId, int page, int pageSize, CancellationToken ct = default)
    {
        // 1. Buat base query (jangan dieksekusi dulu)
        var query = dbContext.Transactions
            .Include(t => t.Account)
            .Include(t => t.Category)
            .Where(t => t.UserId == userId)
            .OrderByDescending(t => t.Date) // Urutkan dari yang terbaru
            .ThenByDescending(t => t.CreatedAt);

        // 2. Hitung total semua data yang ada (sebelum dipotong)
        var totalCount = await query.CountAsync(ct);

        // 3. Potong data sesuai halaman yang diminta
        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return (items, totalCount);
    }

    public async Task AddAsync(Transaction transaction, CancellationToken ct = default)
    {
        await dbContext.Transactions.AddAsync(transaction, ct);
    }

    public void Update(Transaction transaction)
    {
        dbContext.Transactions.Update(transaction);
    }

    public void Delete(Transaction transaction)
    {
        dbContext.Transactions.Remove(transaction);
    }

    public async Task SaveChangesAsync(CancellationToken ct = default)
    {
        await dbContext.SaveChangesAsync(ct);
    }

    public async Task<List<Transaction>> GetByDateRangeAsync(Guid userId, DateTime startDate, DateTime endDate, CancellationToken ct = default)
    {
        return await dbContext.Transactions
            .Include(t => t.Category)
            .Include(t => t.Account)
            .Where(t => t.UserId == userId && t.Date >= startDate && t.Date <= endDate)
            .ToListAsync(ct);
    }
}