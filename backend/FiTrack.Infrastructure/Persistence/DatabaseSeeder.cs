using FiTrack.Domain.Entities;
using FiTrack.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace FiTrack.Infrastructure.Persistence;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(AppDbContext context)
    {
        if (await context.Users.AnyAsync()) return;

        var defaultUser = User.Create(
            "admin",
            "admin@fitrack.com",
            BCrypt.Net.BCrypt.HashPassword("admin123")
        );

        context.Users.Add(defaultUser);
        await context.SaveChangesAsync();

        var defaultAccount = Account.Create(
            defaultUser.Id,
            "Main Bank",
            AccountType.Bank,
            5000000m
        );
        context.Accounts.Add(defaultAccount);

        var foodCategory = Category.Create(
            defaultUser.Id,
            "Food & Dining",
            CategoryType.Expense
        );
        context.Categories.Add(foodCategory);

        var salaryCategory = Category.Create(
            defaultUser.Id,
            "Salary",
            CategoryType.Income
        );
        context.Categories.Add(salaryCategory);

        await context.SaveChangesAsync();

        var transaction = Transaction.Create(
            defaultUser.Id,
            defaultAccount.Id,
            salaryCategory.Id,
            TransactionType.Income,
            5000000m,
            "Monthly Salary",
            DateTime.UtcNow
        );
        context.Transactions.Add(transaction);

        await context.SaveChangesAsync();
    }
}