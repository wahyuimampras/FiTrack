using System;
using FiTrack.Domain.Exceptions;

namespace FiTrack.Domain.Entities;

public class Budget
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public Guid CategoryId { get; private set; }
    public decimal Amount { get; private set; }
    public short Month { get; private set; }
    public short Year { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public User User { get; private set; }
    public Category Category { get; private set; }

    private Budget() { }

    public static Budget Create(Guid userId, Guid categoryId, decimal amount, short month, short year)
    {
        if (amount < 0) throw new DomainException("Amount cannot be negative");
        if (month < 1 || month > 12) throw new DomainException("Invalid month");

        return new Budget
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            CategoryId = categoryId,
            Amount = amount,
            Month = month,
            Year = year,
            CreatedAt = DateTime.UtcNow
        };
    }
}
