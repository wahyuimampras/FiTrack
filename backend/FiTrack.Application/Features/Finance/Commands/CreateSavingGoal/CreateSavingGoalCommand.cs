using System;
using MediatR;

namespace FiTrack.Application.Features.Finance.Commands.CreateSavingGoal;

public record CreateSavingGoalCommand(string Name, decimal TargetAmount, DateTime? TargetDate) : IRequest<Guid>;