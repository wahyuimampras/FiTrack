using System;
using FiTrack.Domain.Enums;
using MediatR;

namespace FiTrack.Application.Features.Finance.Commands.UpdateAccount;

// Kita menggunakan IRequest<Unit> karena proses update tidak perlu mengembalikan data, cukup status sukses (204 No Content).
public record UpdateAccountCommand(
    Guid Id,
    string Name,
    AccountType Type,
    decimal Balance,
    string? Color,
    string? Icon,
    bool IsActive
) : IRequest<Unit>;