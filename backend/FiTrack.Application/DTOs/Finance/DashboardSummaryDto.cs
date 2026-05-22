namespace FiTrack.Application.DTOs.Finance;

public class DashboardSummaryDto
{
    public decimal TotalIncome { get; set; }
    public decimal TotalExpense { get; set; }
    public int TotalActivities { get; set; }
    public float TotalDistanceKm { get; set; }
    public int TotalCaloriesBurned { get; set; }
    public List<CategoryExpenseDto> ExpenseByCategory { get; set; } = [];
    public List<ActivitySummaryDto> ActivityByType { get; set; } = [];
}
