using System.Collections.Generic;
using FiTrack.Application.DTOs.Finance;
using MediatR;

namespace FiTrack.Application.Features.Finance.Queries.GetSavingGoals;

public record GetSavingGoalsQuery() : IRequest<IEnumerable<SavingGoalDto>>;