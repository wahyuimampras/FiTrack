using System;
using FiTrack.Application.DTOs.Finance;
using MediatR;

namespace FiTrack.Application.Features.Finance.Queries.GetSavingGoalById;

public record GetSavingGoalByIdQuery(Guid Id) : IRequest<SavingGoalDto>;