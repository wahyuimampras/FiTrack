using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FiTrack.Domain.Entities;

namespace FiTrack.Domain.Interfaces.Repositories;

public interface ITransactionRepository
{
    Task<Transaction?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<IEnumerable<Transaction>> GetUserTransactionsAsync(Guid userId, CancellationToken ct = default);
    
    // --- TAMBAHKAN METHOD INI UNTUK PAGINASI ---
    Task<(IEnumerable<Transaction> Items, int TotalCount)> GetPagedUserTransactionsAsync(Guid userId, int page, int pageSize, FiTrack.Domain.Enums.TransactionType? type = null, CancellationToken ct = default);
    
    Task<(decimal TotalIncome, decimal TotalExpense)> GetTotalSummaryAsync(Guid userId, CancellationToken ct = default);
    
    Task AddAsync(Transaction transaction, CancellationToken ct = default);
    void Update(Transaction transaction);
    void Delete(Transaction transaction);
    Task SaveChangesAsync(CancellationToken ct = default);
    Task<List<Transaction>> GetByDateRangeAsync(Guid userId, DateTime startDate, DateTime endDate, CancellationToken ct = default);
}