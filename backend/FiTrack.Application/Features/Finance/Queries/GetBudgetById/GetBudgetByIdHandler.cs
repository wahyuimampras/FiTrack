using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FiTrack.Application.DTOs.Finance;
using FiTrack.Application.Interfaces;
using FiTrack.Domain.Interfaces.Repositories;
using MediatR;

namespace FiTrack.Application.Features.Finance.Queries.GetBudgetById;

public class GetBudgetByIdHandler(
    IBudgetRepository budgetRepository,
    ICurrentUserService currentUserService,
    IMapper mapper) : IRequestHandler<GetBudgetByIdQuery, BudgetDto>
{
    public async Task<BudgetDto> Handle(GetBudgetByIdQuery request, CancellationToken cancellationToken)
    {
        var budget = await budgetRepository.GetByIdAsync(request.Id, currentUserService.UserId, cancellationToken);
        
        if (budget == null)
        {
            throw new KeyNotFoundException("Budget tidak ditemukan.");
        }

        return mapper.Map<BudgetDto>(budget);
    }
}