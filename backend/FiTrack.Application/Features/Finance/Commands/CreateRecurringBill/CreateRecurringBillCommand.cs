using MediatR;

namespace FiTrack.Application.Features.Finance.Commands.CreateRecurringBill;

public record CreateRecurringBillCommand(string Name, decimal Amount, short DueDay, Guid? CategoryId) : IRequest<Guid>;