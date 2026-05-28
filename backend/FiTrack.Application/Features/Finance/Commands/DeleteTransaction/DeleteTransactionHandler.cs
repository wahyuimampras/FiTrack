using FiTrack.Application.Interfaces;
using FiTrack.Domain.Enums;
using FiTrack.Domain.Exceptions;
using FiTrack.Domain.Interfaces.Repositories;
using MediatR;

namespace FiTrack.Application.Features.Finance.Commands.DeleteTransaction;

public class DeleteTransactionHandler(
    ITransactionRepository transactionRepository,
    IAccountRepository accountRepository,
    ICurrentUserService currentUserService) : IRequestHandler<DeleteTransactionCommand>
{
    public async Task Handle(DeleteTransactionCommand request, CancellationToken cancellationToken)
    {
        var transaction = await transactionRepository.GetByIdAsync(request.Id, cancellationToken);
        
        if (transaction == null || transaction.UserId != currentUserService.UserId)
            throw new DomainException("Transaction not found.");

        var account = await accountRepository.GetByIdAsync(transaction.AccountId, currentUserService.UserId, cancellationToken);
        if (account != null)
        {
            decimal amountDiff = transaction.Type == TransactionType.Income ? -transaction.Amount : transaction.Amount;
            account.UpdateBalance(amountDiff);
        }

        transactionRepository.Delete(transaction);
        await transactionRepository.SaveChangesAsync(cancellationToken);
    }
}
