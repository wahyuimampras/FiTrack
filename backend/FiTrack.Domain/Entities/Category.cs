using System;
using System.Collections.Generic;
using FiTrack.Domain.Enums;

namespace FiTrack.Domain.Entities;

public class Category
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public string Name { get; private set; }
    public CategoryType Type { get; private set; }
    public string? Icon { get; private set; }
    public string? Color { get; private set; }
    public bool IsDefault { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public User User { get; private set; }
    public ICollection<Transaction> Transactions { get; private set; } = [];
    public ICollection<Budget> Budgets { get; private set; } = [];
    public ICollection<RecurringBill> RecurringBills { get; private set; } = [];

    private Category() { }

    public static Category Create(Guid userId, string name, CategoryType type, string? icon = null, string? color = null, bool isDefault = false)
    {
        return new Category
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Name = name,
            Type = type,
            Icon = icon,
            Color = color,
            IsDefault = isDefault,
            CreatedAt = DateTime.UtcNow
        };
    }

    public void Update(string name, string? icon, string? color)
    {
        Name = name;
        Icon = icon;
        Color = color;
    }
}
