using FiTrack.Application.DTOs.Finance;
using MediatR;

namespace FiTrack.Application.Features.Finance.Queries.GetTransactions;

public record GetTransactionsQuery : IRequest<IEnumerable<TransactionDto>>;
