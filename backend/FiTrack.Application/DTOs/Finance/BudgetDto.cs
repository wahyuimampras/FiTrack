using System;

namespace FiTrack.Application.DTOs.Finance;

public record BudgetDto(Guid Id, Guid CategoryId, string CategoryName, short Month, short Year, decimal Amount);