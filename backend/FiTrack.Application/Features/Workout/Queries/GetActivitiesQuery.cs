using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FiTrack.Application.DTOs.Workout;
using FiTrack.Application.Interfaces;
using FiTrack.Domain.Interfaces.Repositories;
using MediatR;

namespace FiTrack.Application.Features.Workout.Queries;

public class GetActivitiesQuery : IRequest<IEnumerable<ActivityDto>>
{
}

public class GetActivitiesQueryHandler : IRequestHandler<GetActivitiesQuery, IEnumerable<ActivityDto>>
{
    private readonly IActivityRepository _activityRepository;
    private readonly ICurrentUserService _currentUserService;

    public GetActivitiesQueryHandler(IActivityRepository activityRepository, ICurrentUserService currentUserService)
    {
        _activityRepository = activityRepository;
        _currentUserService = currentUserService;
    }

    public async Task<IEnumerable<ActivityDto>> Handle(GetActivitiesQuery request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId;
        if (userId == Guid.Empty)
        {
            throw new UnauthorizedAccessException("User is not authenticated.");
        }

        var activities = await _activityRepository.GetByUserIdAsync(userId, cancellationToken);

        return activities.Select(a => new ActivityDto
        {
            Id = a.Id,
            StravaActivityId = a.StravaActivityId,
            Type = a.Type.ToString(),
            Name = a.Name,
            DistanceMeters = a.DistanceMeters,
            DurationSeconds = a.DurationSeconds,
            ElevationGainMeters = a.ElevationGainMeters,
            CaloriesBurned = a.CaloriesBurned,
            AveragePace = a.AveragePace,
            AverageHeartRate = a.AverageHeartRate,
            StartDate = a.StartDate,
            IsFromStrava = a.IsFromStrava,
            CreatedAt = a.CreatedAt
        }).OrderByDescending(a => a.StartDate);
    }
}
