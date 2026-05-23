using FiTrack.Application.DTOs.Finance;
using MediatR;

namespace FiTrack.Application.Features.Finance.Queries.GetBudgets;

public record GetBudgetsQuery(short? Month, short? Year) : IRequest<IEnumerable<BudgetDto>>;