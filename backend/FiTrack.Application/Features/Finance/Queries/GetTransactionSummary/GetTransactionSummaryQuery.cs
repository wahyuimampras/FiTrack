using MediatR;

namespace FiTrack.Application.Features.Finance.Queries.GetTransactionSummary;

public record TransactionSummaryDto(decimal TotalIncome, decimal TotalExpense);

public record GetTransactionSummaryQuery() : IRequest<TransactionSummaryDto>;