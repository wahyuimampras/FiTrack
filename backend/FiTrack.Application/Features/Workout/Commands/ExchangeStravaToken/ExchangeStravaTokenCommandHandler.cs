using System;
using System.Threading;
using System.Threading.Tasks;
using FiTrack.Application.Interfaces;
using FiTrack.Domain.Interfaces.Services;
using MediatR;

namespace FiTrack.Application.Features.Workout.Commands.ExchangeStravaToken;

public class ExchangeStravaTokenCommandHandler : IRequestHandler<ExchangeStravaTokenCommand, bool>
{
    private readonly IStravaService _stravaService;
    private readonly ICurrentUserService _currentUserService;

    public ExchangeStravaTokenCommandHandler(IStravaService stravaService, ICurrentUserService currentUserService)
    {
        _stravaService = stravaService;
        _currentUserService = currentUserService;
    }

    public async Task<bool> Handle(ExchangeStravaTokenCommand request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId;
        if (userId == Guid.Empty)
        {
            throw new UnauthorizedAccessException("User is not authenticated.");
        }

        await _stravaService.ExchangeCodeAsync(userId, request.Code, cancellationToken);
        return true;
    }
}