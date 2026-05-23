using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FiTrack.Application.DTOs.Finance;
using FiTrack.Application.Interfaces;
using FiTrack.Domain.Interfaces.Repositories;
using MediatR;

namespace FiTrack.Application.Features.Finance.Queries.GetSavingGoals;

public class GetSavingGoalsHandler(
    ISavingGoalRepository savingGoalRepository,
    ICurrentUserService currentUserService,
    IMapper mapper) : IRequestHandler<GetSavingGoalsQuery, IEnumerable<SavingGoalDto>>
{
    public async Task<IEnumerable<SavingGoalDto>> Handle(GetSavingGoalsQuery request, CancellationToken cancellationToken)
    {
        var userId = currentUserService.UserId;
        if (userId == Guid.Empty)
        {
            throw new UnauthorizedAccessException("User is not authenticated.");
        }

        var savingGoals = await savingGoalRepository.GetByUserIdAsync(userId, cancellationToken);

        return mapper.Map<IEnumerable<SavingGoalDto>>(savingGoals);
    }
}