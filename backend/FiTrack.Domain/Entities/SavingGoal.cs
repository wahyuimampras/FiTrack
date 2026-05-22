using System;
using FiTrack.Domain.Exceptions;

namespace FiTrack.Domain.Entities;

public class SavingGoal
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public string Name { get; private set; }
    public decimal TargetAmount { get; private set; }
    public decimal CurrentAmount { get; private set; }
    public DateTime? TargetDate { get; private set; }
    public bool IsCompleted { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public User User { get; private set; }

    private SavingGoal() { }

    public static SavingGoal Create(Guid userId, string name, decimal targetAmount, DateTime? targetDate = null)
    {
        if (targetAmount <= 0) throw new DomainException("Target amount must be positive");

        return new SavingGoal
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Name = name,
            TargetAmount = targetAmount,
            CurrentAmount = 0,
            TargetDate = targetDate,
            IsCompleted = false,
            CreatedAt = DateTime.UtcNow
        };
    }

    public void AddProgress(decimal amount)
    {
        CurrentAmount += amount;
        if (CurrentAmount >= TargetAmount)
        {
            IsCompleted = true;
        }
    }
}
