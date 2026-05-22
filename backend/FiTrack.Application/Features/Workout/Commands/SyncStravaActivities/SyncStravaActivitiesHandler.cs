using MediatR;
using FiTrack.Domain.Entities;
using FiTrack.Domain.Interfaces.Services;
using FiTrack.Application.Interfaces;

namespace FiTrack.Application.Features.Workout.Commands.SyncStravaActivities;

public class SyncStravaActivitiesHandler(
    IStravaService stravaService,
    ICurrentUserService currentUser
) : IRequestHandler<SyncStravaActivitiesCommand, List<Activity>>
{
    public async Task<List<Activity>> Handle(SyncStravaActivitiesCommand request, CancellationToken ct)
    {
        var userId = currentUser.UserId;
        var activities = await stravaService.SyncActivitiesAsync(userId, 1, 30, ct);
        return activities;
    }
}