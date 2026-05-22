using AutoMapper;
using FiTrack.Application.DTOs.Finance;
using FiTrack.Application.Interfaces;
using FiTrack.Domain.Exceptions;
using FiTrack.Domain.Interfaces.Repositories;
using MediatR;

namespace FiTrack.Application.Features.Finance.Queries.GetTransactionById;

public class GetTransactionByIdHandler(
    ITransactionRepository transactionRepository,
    ICurrentUserService currentUserService,
    IMapper mapper) : IRequestHandler<GetTransactionByIdQuery, TransactionDto>
{
    public async Task<TransactionDto> Handle(GetTransactionByIdQuery request, CancellationToken cancellationToken)
    {
        var transaction = await transactionRepository.GetByIdAsync(request.Id, cancellationToken);
        
        if (transaction == null || transaction.UserId != currentUserService.UserId)
            throw new DomainException("Transaction not found.");

        return mapper.Map<TransactionDto>(transaction);
    }
}
