using MediatR;

namespace FiTrack.Application.Features.Finance.Commands.CreateBudget;

public record CreateBudgetCommand(Guid CategoryId, decimal Amount, short Month, short Year) : IRequest<Guid>;