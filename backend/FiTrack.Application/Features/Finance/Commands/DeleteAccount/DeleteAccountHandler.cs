using FiTrack.Application.Interfaces;
using FiTrack.Domain.Interfaces.Repositories;
using MediatR;

namespace FiTrack.Application.Features.Finance.Commands.DeleteAccount;

public class DeleteAccountHandler(
    IAccountRepository accountRepository,
    ICurrentUserService currentUserService) : IRequestHandler<DeleteAccountCommand, Unit>
{
    public async Task<Unit> Handle(DeleteAccountCommand request, CancellationToken cancellationToken)
    {
        var userId = currentUserService.UserId;
        // Validasi Opsi 1
        if (userId == Guid.Empty)
        {
            throw new UnauthorizedAccessException("User is not authenticated.");
        }

        var account = await accountRepository.GetByIdAsync(request.Id, userId, cancellationToken);
        if (account == null)
        {
            throw new KeyNotFoundException($"Account with ID {request.Id} not found.");
        }

        await accountRepository.DeleteAsync(account, cancellationToken);
        await accountRepository.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}