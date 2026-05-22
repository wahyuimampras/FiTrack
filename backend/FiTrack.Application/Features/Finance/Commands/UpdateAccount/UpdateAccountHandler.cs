using FiTrack.Application.Interfaces;
using FiTrack.Domain.Interfaces.Repositories;
using MediatR;

namespace FiTrack.Application.Features.Finance.Commands.UpdateAccount;

public class UpdateAccountHandler(
    IAccountRepository accountRepository,
    ICurrentUserService currentUserService) : IRequestHandler<UpdateAccountCommand, Unit>
{
    public async Task<Unit> Handle(UpdateAccountCommand request, CancellationToken cancellationToken)
    {
        var userId = currentUserService.UserId;
        if (userId == Guid.Empty)
        {
            throw new UnauthorizedAccessException("User is not authenticated.");
        }

        var account = await accountRepository.GetByIdAsync(request.Id, userId, cancellationToken);
        if (account == null)
        {
            throw new KeyNotFoundException($"Account with ID {request.Id} not found.");
        }

        // HAPUS atau COMMENT kode yang lama:
        // account.Name = request.Name;
        // account.Type = request.Type; ... dsb.

        // GANTI dengan pemanggilan method ini:
        account.UpdateDetails(
            request.Name, 
            request.Type, 
            request.Balance, 
            request.Color, 
            request.Icon, 
            request.IsActive);

        await accountRepository.UpdateAsync(account, cancellationToken);
        await accountRepository.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}