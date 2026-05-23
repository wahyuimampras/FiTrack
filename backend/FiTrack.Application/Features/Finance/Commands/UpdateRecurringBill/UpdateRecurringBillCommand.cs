using MediatR;

namespace FiTrack.Application.Features.Finance.Commands.UpdateRecurringBill;

public record UpdateRecurringBillCommand(Guid Id, string Name, decimal Amount, short DueDay, Guid? CategoryId, bool IsActive) : IRequest<Unit>;