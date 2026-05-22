using FiTrack.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FiTrack.Infrastructure.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Transaction> Transactions => Set<Transaction>();
    public DbSet<Account> Accounts => Set<Account>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Budget> Budgets => Set<Budget>();
    public DbSet<SavingGoal> SavingGoals => Set<SavingGoal>();
    public DbSet<RecurringBill> RecurringBills => Set<RecurringBill>();
    public DbSet<Activity> Activities => Set<Activity>();
    public DbSet<StravaToken> StravaTokens => Set<StravaToken>();
    public DbSet<UserSession> UserSessions => Set<UserSession>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}