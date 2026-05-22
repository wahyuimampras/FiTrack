using FiTrack.Application.Interfaces;
using FiTrack.Domain.Exceptions;
using FiTrack.Domain.Interfaces.Repositories;
using MediatR;

namespace FiTrack.Application.Features.Finance.Commands.DeleteTransaction;

public class DeleteTransactionHandler(
    ITransactionRepository transactionRepository,
    ICurrentUserService currentUserService) : IRequestHandler<DeleteTransactionCommand>
{
    public async Task Handle(DeleteTransactionCommand request, CancellationToken cancellationToken)
    {
        var transaction = await transactionRepository.GetByIdAsync(request.Id, cancellationToken);
        
        if (transaction == null || transaction.UserId != currentUserService.UserId)
            throw new DomainException("Transaction not found.");

        transactionRepository.Delete(transaction);
        await transactionRepository.SaveChangesAsync(cancellationToken);
    }
}
