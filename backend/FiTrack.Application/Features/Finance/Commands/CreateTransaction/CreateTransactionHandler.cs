using MediatR;
using AutoMapper;
using FiTrack.Domain.Entities;
using FiTrack.Domain.Enums;
using FiTrack.Domain.Exceptions;
using FiTrack.Domain.Interfaces.Repositories;
using FiTrack.Application.Interfaces;
using FiTrack.Application.DTOs.Finance;

namespace FiTrack.Application.Features.Finance.Commands.CreateTransaction;

public class CreateTransactionHandler(
    ITransactionRepository repo,
    IAccountRepository accountRepo,
    ICurrentUserService currentUser,
    IMapper mapper
) : IRequestHandler<CreateTransactionCommand, TransactionDto>
{
    public async Task<TransactionDto> Handle(
        CreateTransactionCommand request,
        CancellationToken ct)
    {
        var account = await accountRepo.GetByIdAsync(request.AccountId, currentUser.UserId, ct);
        if (account == null) throw new DomainException("Account not found.");

        var transaction = Transaction.Create(
            currentUser.UserId,
            request.AccountId,
            request.CategoryId,
            request.Type,
            request.Amount,
            request.Description,
            request.Date,
            request.Notes
        );

        decimal amountDiff = request.Type == TransactionType.Income ? request.Amount : -request.Amount;
        account.UpdateBalance(amountDiff);

        await repo.AddAsync(transaction, ct);
        await repo.SaveChangesAsync(ct); // This should also save the account balance change since they share the same DbContext

        return mapper.Map<TransactionDto>(transaction);
    }
}