using AutoMapper;
using FiTrack.Application.DTOs.Finance;
using FiTrack.Application.Interfaces;
using FiTrack.Domain.Exceptions;
using FiTrack.Domain.Interfaces.Repositories;
using MediatR;

namespace FiTrack.Application.Features.Finance.Commands.UpdateTransaction;

public class UpdateTransactionHandler(
    ITransactionRepository transactionRepository,
    ICurrentUserService currentUserService,
    IMapper mapper) : IRequestHandler<UpdateTransactionCommand, TransactionDto>
{
    public async Task<TransactionDto> Handle(UpdateTransactionCommand request, CancellationToken cancellationToken)
    {
        var transaction = await transactionRepository.GetByIdAsync(request.Id, cancellationToken);
        
        if (transaction == null || transaction.UserId != currentUserService.UserId)
            throw new DomainException("Transaction not found.");

        transaction.Update(
            request.AccountId,
            request.CategoryId,
            request.Type,
            request.Amount,
            request.Description,
            request.Date,
            request.Notes
        );

        transactionRepository.Update(transaction);
        await transactionRepository.SaveChangesAsync(cancellationToken);

        // Fetch again to include properties like Account/Category names if needed, 
        // or just rely on mapping what we have.
        var updatedTransaction = await transactionRepository.GetByIdAsync(request.Id, cancellationToken);
        return mapper.Map<TransactionDto>(updatedTransaction);
    }
}
