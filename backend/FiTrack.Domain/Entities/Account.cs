using System;
using System.Collections.Generic;
using FiTrack.Domain.Enums;

namespace FiTrack.Domain.Entities;

public class Account
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public string Name { get; private set; }
    public AccountType Type { get; private set; }
    public decimal Balance { get; private set; }
    public string? Color { get; private set; }
    public string? Icon { get; private set; }
    public bool IsActive { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public User User { get; private set; }
    public ICollection<Transaction> Transactions { get; private set; } = [];

    private Account() { }

    public static Account Create(Guid userId, string name, AccountType type, decimal initialBalance = 0, string? color = null, string? icon = null)
    {
        return new Account
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Name = name,
            Type = type,
            Balance = initialBalance,
            Color = color,
            Icon = icon,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };
    }

    public void UpdateBalance(decimal amount)
    {
        Balance += amount;
    }

    public void UpdateDetails(string name, AccountType type, decimal balance, string? color, string? icon, bool isActive)
    {
        Name = name;
        Type = type;
        Balance = balance;
        Color = color;
        Icon = icon;
        IsActive = isActive;
    }
}
