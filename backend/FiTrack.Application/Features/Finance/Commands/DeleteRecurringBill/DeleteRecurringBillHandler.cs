using FiTrack.Application.Interfaces;
using FiTrack.Domain.Interfaces.Repositories;
using MediatR;

namespace FiTrack.Application.Features.Finance.Commands.DeleteRecurringBill;

public class DeleteRecurringBillHandler(
    IRecurringBillRepository recurringBillRepository,
    ICurrentUserService currentUserService) : IRequestHandler<DeleteRecurringBillCommand, Unit>
{
    public async Task<Unit> Handle(DeleteRecurringBillCommand request, CancellationToken cancellationToken)
    {
        var userId = currentUserService.UserId;
        if (userId == Guid.Empty) throw new UnauthorizedAccessException("User is not authenticated.");

        var bill = await recurringBillRepository.GetByIdAsync(request.Id, userId, cancellationToken);
        if (bill == null) throw new KeyNotFoundException("Recurring bill not found.");

        await recurringBillRepository.DeleteAsync(bill, cancellationToken);
        await recurringBillRepository.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}