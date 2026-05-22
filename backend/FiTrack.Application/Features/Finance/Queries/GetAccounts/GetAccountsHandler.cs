// GetAccountsHandler.cs
using AutoMapper;
using FiTrack.Application.DTOs.Finance;
using FiTrack.Application.Interfaces;
using FiTrack.Domain.Interfaces.Repositories;
using MediatR;

namespace FiTrack.Application.Features.Finance.Queries.GetAccounts;

public class GetAccountsHandler(
    IAccountRepository accountRepository,
    ICurrentUserService currentUserService,
    IMapper mapper) : IRequestHandler<GetAccountsQuery, IEnumerable<AccountDto>>
{
    public async Task<IEnumerable<AccountDto>> Handle(GetAccountsQuery request, CancellationToken cancellationToken)
    {
        var userId = currentUserService.UserId;
        if (userId == Guid.Empty)
        {
            throw new UnauthorizedAccessException("User is not authenticated.");
        }

        var accounts = await accountRepository.GetByUserIdAsync(userId, cancellationToken);
        
        return mapper.Map<IEnumerable<AccountDto>>(accounts);
    }
}