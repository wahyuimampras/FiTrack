using FiTrack.Application.Interfaces;
using FiTrack.Domain.Entities;
using FiTrack.Domain.Interfaces.Repositories;
using MediatR;

namespace FiTrack.Application.Features.Finance.Commands.CreateAccount;

public class CreateAccountHandler(
    IAccountRepository accountRepository,
    ICurrentUserService currentUserService) : IRequestHandler<CreateAccountCommand, Guid>
{
    public async Task<Guid> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
    {
        var userId = currentUserService.UserId;
        if (userId == Guid.Empty)
        {
            throw new UnauthorizedAccessException("User is not authenticated.");
        }

        var account = Account.Create(
            userId,
            request.Name,
            request.Type,
            request.InitialBalance,
            request.Color,
            request.Icon);

        await accountRepository.AddAsync(account, cancellationToken);
        await accountRepository.SaveChangesAsync(cancellationToken);

        return account.Id;
    }
}