using System;
using System.Collections.Generic;

namespace FiTrack.Domain.Entities;

public class User
{
    public Guid Id { get; private set; }
    public string Username { get; private set; }
    public string Email { get; private set; }
    public string PasswordHash { get; private set; }
    public string? StravaAthleteId { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public bool IsActive { get; private set; }

    public ICollection<UserSession> Sessions { get; private set; } = [];
    public ICollection<Transaction> Transactions { get; private set; } = [];
    public ICollection<Activity> Activities { get; private set; } = [];
    public ICollection<Account> Accounts { get; private set; } = [];
    public ICollection<Category> Categories { get; private set; } = [];
    public ICollection<Budget> Budgets { get; private set; } = [];
    public ICollection<SavingGoal> SavingGoals { get; private set; } = [];
    public ICollection<RecurringBill> RecurringBills { get; private set; } = [];
    public StravaToken? StravaToken { get; private set; }

    private User() { }

    public static User Create(string username, string email, string passwordHash)
    {
        return new User
        {
            Id = Guid.NewGuid(),
            Username = username,
            Email = email.ToLowerInvariant(),
            PasswordHash = passwordHash,
            CreatedAt = DateTime.UtcNow,
            IsActive = true
        };
    }

    public void ConnectStrava(string athleteId)
    {
        StravaAthleteId = athleteId;
    }

    public void DisconnectStrava()
    {
        StravaAthleteId = null;
    }
}
