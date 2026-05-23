using MediatR;

namespace FiTrack.Application.Features.Finance.Commands.UpdateBudget;

public record UpdateBudgetCommand(Guid Id, Guid CategoryId, decimal Amount, short Month, short Year) : IRequest<Unit>;