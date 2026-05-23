using System.Collections.Generic;

namespace FiTrack.Application.DTOs.Finance;

public class MonthlyReportDto
{
    public int Month { get; set; }
    public int Year { get; set; }
    public decimal TotalIncome { get; set; }
    public decimal TotalExpense { get; set; }
    public decimal NetSavings => TotalIncome - TotalExpense;
    public List<CategoryExpenseDto> ExpenseByCategory { get; set; } = new();
    public List<TransactionDto> Transactions { get; set; } = new();
}