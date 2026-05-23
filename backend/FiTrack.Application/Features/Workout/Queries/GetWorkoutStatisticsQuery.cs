using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FiTrack.Application.DTOs.Workout;
using FiTrack.Application.Interfaces;
using FiTrack.Domain.Interfaces.Repositories;
using MediatR;

namespace FiTrack.Application.Features.Workout.Queries;

public class GetWorkoutStatisticsQuery : IRequest<WorkoutStatisticsDto>
{
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}

public class GetWorkoutStatisticsQueryHandler : IRequestHandler<GetWorkoutStatisticsQuery, WorkoutStatisticsDto>
{
    private readonly IActivityRepository _activityRepository;
    private readonly ICurrentUserService _currentUserService;

    public GetWorkoutStatisticsQueryHandler(IActivityRepository activityRepository, ICurrentUserService currentUserService)
    {
        _activityRepository = activityRepository;
        _currentUserService = currentUserService;
    }

    public async Task<WorkoutStatisticsDto> Handle(GetWorkoutStatisticsQuery request, CancellationToken cancellationToken)
    {
        var userId = _currentUserService.UserId;
        if (userId == Guid.Empty)
        {
            throw new UnauthorizedAccessException("User is not authenticated.");
        }

        var activities = await _activityRepository.GetByUserIdAsync(userId, cancellationToken);
        
        var filteredActivities = activities.AsQueryable();

        if (request.StartDate.HasValue)
        {
            filteredActivities = filteredActivities.Where(a => a.StartDate >= request.StartDate.Value);
        }

        if (request.EndDate.HasValue)
        {
            filteredActivities = filteredActivities.Where(a => a.StartDate <= request.EndDate.Value);
        }

        var activitiesList = filteredActivities.ToList();

        return new WorkoutStatisticsDto
        {
            TotalActivities = activitiesList.Count,
            TotalDistanceMeters = activitiesList.Sum(a => a.DistanceMeters),
            TotalDurationSeconds = activitiesList.Sum(a => a.DurationSeconds),
            TotalCaloriesBurned = activitiesList.Sum(a => a.CaloriesBurned ?? 0),
            TotalElevationGainMeters = activitiesList.Sum(a => a.ElevationGainMeters ?? 0)
        };
    }
}
