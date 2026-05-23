using FiTrack.Application.Interfaces;
using FiTrack.Domain.Interfaces.Repositories;
using MediatR;

namespace FiTrack.Application.Features.Finance.Commands.UpdateRecurringBill;

public class UpdateRecurringBillHandler(
    IRecurringBillRepository recurringBillRepository,
    ICurrentUserService currentUserService) : IRequestHandler<UpdateRecurringBillCommand, Unit>
{
    public async Task<Unit> Handle(UpdateRecurringBillCommand request, CancellationToken cancellationToken)
    {
        var userId = currentUserService.UserId;
        if (userId == Guid.Empty) throw new UnauthorizedAccessException("User is not authenticated.");

        var bill = await recurringBillRepository.GetByIdAsync(request.Id, userId, cancellationToken);
        if (bill == null) throw new KeyNotFoundException("Recurring bill not found.");

        bill.Update(request.Name, request.Amount, request.DueDay, request.CategoryId, request.IsActive);

        await recurringBillRepository.UpdateAsync(bill, cancellationToken);
        await recurringBillRepository.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}