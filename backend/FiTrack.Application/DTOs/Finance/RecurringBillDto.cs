using System;

namespace FiTrack.Application.DTOs.Finance;

public record RecurringBillDto(
    Guid Id,
    Guid? CategoryId,
    string? CategoryName,
    string Name,
    decimal Amount,
    short DueDay,
    bool IsActive
);