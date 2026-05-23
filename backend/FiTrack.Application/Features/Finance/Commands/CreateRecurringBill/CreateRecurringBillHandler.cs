using FiTrack.Application.Interfaces;
using FiTrack.Domain.Entities;
using FiTrack.Domain.Interfaces.Repositories;
using MediatR;

namespace FiTrack.Application.Features.Finance.Commands.CreateRecurringBill;

public class CreateRecurringBillHandler(
    IRecurringBillRepository recurringBillRepository,
    ICurrentUserService currentUserService) : IRequestHandler<CreateRecurringBillCommand, Guid>
{
    public async Task<Guid> Handle(CreateRecurringBillCommand request, CancellationToken cancellationToken)
    {
        var userId = currentUserService.UserId;
        if (userId == Guid.Empty) throw new UnauthorizedAccessException("User is not authenticated.");

        var bill = RecurringBill.Create(userId, request.Name, request.Amount, request.DueDay, request.CategoryId);

        await recurringBillRepository.AddAsync(bill, cancellationToken);
        await recurringBillRepository.SaveChangesAsync(cancellationToken);

        return bill.Id;
    }
}