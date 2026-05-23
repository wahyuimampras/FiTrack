using MediatR;

namespace FiTrack.Application.Features.Finance.Commands.DeleteRecurringBill;

public record DeleteRecurringBillCommand(Guid Id) : IRequest<Unit>;