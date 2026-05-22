using System;
using FiTrack.Domain.Enums;
using MediatR;

namespace FiTrack.Application.Features.Finance.Commands.CreateAccount;

public record CreateAccountCommand(
    string Name,
    AccountType Type,
    decimal InitialBalance,
    string? Color,
    string? Icon
) : IRequest<Guid>;