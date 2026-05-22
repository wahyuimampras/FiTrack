using MediatR;
using AutoMapper;
using FiTrack.Domain.Entities;
using FiTrack.Domain.Interfaces.Repositories;
using FiTrack.Application.Interfaces;
using FiTrack.Application.DTOs.Finance;

namespace FiTrack.Application.Features.Finance.Commands.CreateTransaction;

public class CreateTransactionHandler(
    ITransactionRepository repo,
    ICurrentUserService currentUser,
    IMapper mapper
) : IRequestHandler<CreateTransactionCommand, TransactionDto>
{
    public async Task<TransactionDto> Handle(
        CreateTransactionCommand request,
        CancellationToken ct)
    {
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

        await repo.AddAsync(transaction, ct);
        await repo.SaveChangesAsync(ct);

        return mapper.Map<TransactionDto>(transaction);
    }
}