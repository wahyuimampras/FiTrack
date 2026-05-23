using MediatR;

namespace FiTrack.Application.Features.Finance.Commands.DeleteBudget;

public record DeleteBudgetCommand(Guid Id) : IRequest<Unit>;