using MediatR;
using FiTrack.Domain.Enums;
using FiTrack.Application.DTOs.Finance;

namespace FiTrack.Application.Features.Finance.Commands.CreateTransaction;

public record CreateTransactionCommand(
    Guid AccountId,
    Guid CategoryId,
    TransactionType Type,
    decimal Amount,
    string Description,
    DateTime Date,
    string? Notes
) : IRequest<TransactionDto>;