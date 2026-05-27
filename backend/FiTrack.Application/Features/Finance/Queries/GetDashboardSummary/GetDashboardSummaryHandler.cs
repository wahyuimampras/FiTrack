using MediatR;
using FiTrack.Domain.Enums;
using FiTrack.Domain.Interfaces.Repositories;
using FiTrack.Application.Interfaces;
using FiTrack.Application.DTOs.Finance;

namespace FiTrack.Application.Features.Finance.Queries.GetDashboardSummary;

public class GetDashboardSummaryHandler(
    ITransactionRepository transactionRepo,
    IActivityRepository activityRepo,
    ICurrentUserService currentUser
) : IRequestHandler<GetDashboardSummaryQuery, DashboardSummaryDto>
{
    public async Task<DashboardSummaryDto> Handle(
        GetDashboardSummaryQuery request,
        CancellationToken ct)
    {
        var userId = currentUser.UserId;
        var startDate = new DateTime(request.Year, request.Month, 1, 0, 0, 0, DateTimeKind.Utc);
        var endDate = startDate.AddMonths(1).AddTicks(-1);

        var transactions = await transactionRepo.GetByDateRangeAsync(userId, startDate, endDate, ct);
        var activities = await activityRepo.GetByDateRangeAsync(userId, startDate, endDate, ct);

        return new DashboardSummaryDto
        {
            TotalIncome = transactions.Where(t => t.Type == TransactionType.Income).Sum(t => t.Amount),
            TotalExpense = transactions.Where(t => t.Type == TransactionType.Expense).Sum(t => t.Amount),
            TotalActivities = activities.Count,
            TotalDistanceKm = activities.Sum(a => a.DistanceMeters) / 1000f,
            TotalCaloriesBurned = activities.Sum(a => a.CaloriesBurned ?? 0),
            ExpenseByCategory = transactions
                .Where(t => t.Type == TransactionType.Expense)
                .GroupBy(t => t.Category.Name)
                .Select(g => new CategoryExpenseDto { Category = g.Key, Amount = g.Sum(t => t.Amount) })
                .ToList(),
            ActivityByType = activities
                .GroupBy(a => a.Type.ToString())
                .Select(g => new ActivitySummaryDto { Type = g.Key, Count = g.Count() })
                .ToList()
        };
    }
}