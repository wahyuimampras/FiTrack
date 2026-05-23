using System;
using MediatR;

namespace FiTrack.Application.Features.Finance.Commands.DeleteSavingGoal;

public record DeleteSavingGoalCommand(Guid Id) : IRequest<Unit>;