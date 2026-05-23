using FiTrack.Application.DTOs.Finance;
using FiTrack.Application.Interfaces;
using FiTrack.Domain.Interfaces.Repositories;
using MediatR;
using AutoMapper;

namespace FiTrack.Application.Features.Finance.Queries.GetBudgets;

public class GetBudgetsHandler(IBudgetRepository repo, ICurrentUserService user, IMapper mapper) 
    : IRequestHandler<GetBudgetsQuery, IEnumerable<BudgetDto>> 
{
    public async Task<IEnumerable<BudgetDto>> Handle(GetBudgetsQuery req, CancellationToken ct) 
    {
        var list = await repo.GetByUserIdAsync(user.UserId, req.Month, req.Year, ct);
        return mapper.Map<IEnumerable<BudgetDto>>(list);
    }
}   