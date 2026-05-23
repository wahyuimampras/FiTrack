using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FiTrack.Application.DTOs.Finance;
using FiTrack.Application.Interfaces;
using FiTrack.Domain.Interfaces.Repositories;
using MediatR;

namespace FiTrack.Application.Features.Finance.Queries.GetAccountById;

public class GetAccountByIdHandler(
    IAccountRepository accountRepository,
    ICurrentUserService currentUserService,
    IMapper mapper) : IRequestHandler<GetAccountByIdQuery, AccountDto>
{
    public async Task<AccountDto> Handle(GetAccountByIdQuery request, CancellationToken cancellationToken)
    {
        var userId = currentUserService.UserId;
        
        // Panggil GetByIdAsync dari IAccountRepository
        var account = await accountRepository.GetByIdAsync(request.Id, userId, cancellationToken);

        if (account == null)
        {
            throw new KeyNotFoundException("Account tidak ditemukan atau Anda tidak memiliki akses.");
        }

        // Map entitas Account ke AccountDto
        return mapper.Map<AccountDto>(account);
    }
}