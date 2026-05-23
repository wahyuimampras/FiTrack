using System;

namespace FiTrack.Application.DTOs.Finance;

public record SavingGoalDto(
    Guid Id,
    string Name,
    decimal TargetAmount,
    decimal CurrentAmount,
    DateTime? TargetDate,
    bool IsCompleted
);