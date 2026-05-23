using System;
using MediatR;

namespace FiTrack.Application.Features.Finance.Commands.UpdateSavingGoal;

public record UpdateSavingGoalCommand(Guid Id, string Name, decimal TargetAmount, DateTime? TargetDate) : IRequest<Unit>;