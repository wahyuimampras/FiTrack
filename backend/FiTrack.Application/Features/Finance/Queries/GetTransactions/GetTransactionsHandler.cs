using AutoMapper;
using FiTrack.Application.DTOs.Finance;
using FiTrack.Application.Interfaces;
using FiTrack.Domain.Interfaces.Repositories;
using MediatR;

namespace FiTrack.Application.Features.Finance.Queries.GetTransactions;

public class GetTransactionsHandler(
    ITransactionRepository transactionRepository,
    ICurrentUserService currentUserService,
    IMapper mapper) : IRequestHandler<GetTransactionsQuery, IEnumerable<TransactionDto>>
{
    public async Task<IEnumerable<TransactionDto>> Handle(GetTransactionsQuery request, CancellationToken cancellationToken)
    {
        var transactions = await transactionRepository.GetUserTransactionsAsync(currentUserService.UserId, cancellationToken);
        return mapper.Map<IEnumerable<TransactionDto>>(transactions);
    }
}
