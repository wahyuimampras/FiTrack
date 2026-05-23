using System;
using System.Threading;
using System.Threading.Tasks;
using FiTrack.Application.Interfaces;
using FiTrack.Domain.Interfaces.Services;
using MediatR;

namespace FiTrack.Application.Features.Workout.Commands.DisconnectStrava;

public class DisconnectStravaCommandHandler : IRequestHandler<DisconnectStravaCommand, bool>
{
    private readonly IStravaService _stravaService;
    private readonly ICurrentUserService _currentUserService;

    public DisconnectStravaCommandHandler(IStravaService stravaService, ICurrentUserService currentUserService)
    {
        _stravaService = stravaService;
        _currentUserService = currentUserService;
    }

    public async Task<bool> Handle(DisconnectStravaCommand request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId;
        if (userId == Guid.Empty)
        {
            throw new UnauthorizedAccessException("User is not authenticated.");
        }

        await _stravaService.DisconnectAsync(userId, cancellationToken);
        return true;
    }
}