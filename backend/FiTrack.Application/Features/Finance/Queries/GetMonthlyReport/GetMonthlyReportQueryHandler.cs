using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using FiTrack.Application.DTOs.Finance;
using FiTrack.Domain.Enums;
using FiTrack.Domain.Interfaces.Repositories;
using FiTrack.Application.Interfaces;

namespace FiTrack.Application.Features.Finance.Queries.GetMonthlyReport;

public class GetMonthlyReportQueryHandler(
    ITransactionRepository transactionRepo,
    ICurrentUserService currentUser
) : IRequestHandler<GetMonthlyReportQuery, MonthlyReportDto>
{
    public async Task<MonthlyReportDto> Handle(GetMonthlyReportQuery request, CancellationToken cancellationToken)
    {
        var userId = currentUser.UserId;
        var startDate = new DateTime(request.Year, request.Month, 1, 0, 0, 0, DateTimeKind.Utc);
        var endDate = startDate.AddMonths(1).AddTicks(-1);

        var transactions = await transactionRepo.GetByDateRangeAsync(userId, startDate, endDate, cancellationToken);

        return new MonthlyReportDto
        {
            Month = request.Month,
            Year = request.Year,
            TotalIncome = transactions.Where(t => t.Type == TransactionType.Income).Sum(t => t.Amount),
            TotalExpense = transactions.Where(t => t.Type == TransactionType.Expense).Sum(t => t.Amount),
            ExpenseByCategory = transactions
                .Where(t => t.Type == TransactionType.Expense && t.Category != null)
                .GroupBy(t => t.Category.Name)
                .Select(g => new CategoryExpenseDto { Category = g.Key, Amount = g.Sum(t => t.Amount) })
                .ToList(),
            Transactions = transactions.Select(t => new TransactionDto
            {
                Id = t.Id,
                AccountId = t.AccountId,
                CategoryId = t.CategoryId,
                Type = t.Type,
                Amount = t.Amount,
                Description = t.Description,
                Date = t.Date,
                Notes = t.Notes,
                CreatedAt = t.CreatedAt
            }).OrderByDescending(t => t.Date).ToList()
        };
    }
}
