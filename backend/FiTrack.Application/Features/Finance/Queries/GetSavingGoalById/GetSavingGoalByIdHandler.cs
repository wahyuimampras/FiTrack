using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FiTrack.Application.DTOs.Finance;
using FiTrack.Application.Interfaces;
using FiTrack.Domain.Interfaces.Repositories;
using MediatR;

namespace FiTrack.Application.Features.Finance.Queries.GetSavingGoalById;

public class GetSavingGoalByIdHandler(
    ISavingGoalRepository savingGoalRepository,
    ICurrentUserService currentUserService,
    IMapper mapper) : IRequestHandler<GetSavingGoalByIdQuery, SavingGoalDto>
{
    public async Task<SavingGoalDto> Handle(GetSavingGoalByIdQuery request, CancellationToken cancellationToken)
    {
        var userId = currentUserService.UserId;
        if (userId == Guid.Empty)
        {
            throw new UnauthorizedAccessException("User is not authenticated.");
        }

        var savingGoal = await savingGoalRepository.GetByIdAsync(request.Id, userId, cancellationToken);
        if (savingGoal == null)
        {
            throw new KeyNotFoundException("Saving goal not found.");
        }

        return mapper.Map<SavingGoalDto>(savingGoal);
    }
}