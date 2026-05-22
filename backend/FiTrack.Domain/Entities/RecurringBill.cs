using System;
using FiTrack.Domain.Exceptions;

namespace FiTrack.Domain.Entities;

public class RecurringBill
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public Guid? CategoryId { get; private set; }
    public string Name { get; private set; }
    public decimal Amount { get; private set; }
    public short DueDay { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime? LastReminded { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public User User { get; private set; }
    public Category? Category { get; private set; }

    private RecurringBill() { }

    public static RecurringBill Create(Guid userId, string name, decimal amount, short dueDay, Guid? categoryId = null)
    {
        if (amount < 0) throw new DomainException("Amount cannot be negative");
        if (dueDay < 1 || dueDay > 31) throw new DomainException("Due day must be between 1 and 31");

        return new RecurringBill
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            CategoryId = categoryId,
            Name = name,
            Amount = amount,
            DueDay = dueDay,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };
    }
}
