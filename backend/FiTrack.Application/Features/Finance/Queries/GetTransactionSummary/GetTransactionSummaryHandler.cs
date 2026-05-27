using System;
using System.Threading;
using System.Threading.Tasks;
using FiTrack.Application.Interfaces;
using FiTrack.Domain.Interfaces.Repositories;
using MediatR;

namespace FiTrack.Application.Features.Finance.Queries.GetTransactionSummary;

public class GetTransactionSummaryHandler(
    ITransactionRepository transactionRepository,
    ICurrentUserService currentUserService) : IRequestHandler<GetTransactionSummaryQuery, TransactionSummaryDto>
{
    public async Task<TransactionSummaryDto> Handle(GetTransactionSummaryQuery request, CancellationToken cancellationToken)
    {
        var userId = currentUserService.UserId;
        if (userId == Guid.Empty) throw new UnauthorizedAccessException("User is not authenticated.");

        var summary = await transactionRepository.GetTotalSummaryAsync(userId, cancellationToken);

        return new TransactionSummaryDto(summary.TotalIncome, summary.TotalExpense);
    }
}