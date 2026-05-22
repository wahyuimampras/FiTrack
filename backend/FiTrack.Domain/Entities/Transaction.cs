using System;
using FiTrack.Domain.Enums;
using FiTrack.Domain.Exceptions;

namespace FiTrack.Domain.Entities;

public class Transaction
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public Guid AccountId { get; private set; }
    public Guid CategoryId { get; private set; }
    public TransactionType Type { get; private set; }
    public decimal Amount { get; private set; }
    public string Description { get; private set; }
    public DateTime Date { get; private set; }
    public string? Notes { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public User User { get; private set; }
    public Account Account { get; private set; }
    public Category Category { get; private set; }

    private Transaction() { }

    public static Transaction Create(
        Guid userId, Guid accountId, Guid categoryId,
        TransactionType type, decimal amount,
        string description, DateTime date, string? notes = null)
    {
        if (amount <= 0) throw new DomainException("Amount must be positive");

        return new Transaction
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            AccountId = accountId,
            CategoryId = categoryId,
            Type = type,
            Amount = amount,
            Description = description,
            Date = date,
            Notes = notes,
            CreatedAt = DateTime.UtcNow
        };
    }

    public void Update(
        Guid accountId, Guid categoryId,
        TransactionType type, decimal amount,
        string description, DateTime date, string? notes = null)
    {
        if (amount <= 0) throw new DomainException("Amount must be positive");

        AccountId = accountId;
        CategoryId = categoryId;
        Type = type;
        Amount = amount;
        Description = description;
        Date = date;
        Notes = notes;
    }
}
