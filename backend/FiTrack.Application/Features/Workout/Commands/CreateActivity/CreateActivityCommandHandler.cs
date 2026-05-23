using System.Threading;
using System.Threading.Tasks;
using MediatR;
using FiTrack.Domain.Entities;
using FiTrack.Domain.Interfaces.Repositories;
using FiTrack.Application.Interfaces;
using System;
using FiTrack.Domain.Exceptions;

namespace FiTrack.Application.Features.Workout.Commands.CreateActivity;

public class CreateActivityCommandHandler(
    IActivityRepository activityRepository,
    ICurrentUserService currentUserService) : IRequestHandler<CreateActivityCommand, Guid>
{
    public async Task<Guid> Handle(CreateActivityCommand request, CancellationToken cancellationToken)
    {
        var userId = currentUserService.UserId;
        if (userId == Guid.Empty)
        {
            throw new DomainException("User is not authenticated.");
        }

        var activity = Activity.Create(
            userId: userId,
            type: request.Type,
            name: request.Name,
            distanceMeters: request.DistanceMeters,
            durationSeconds: request.DurationSeconds,
            startDate: request.StartDate,
            elevationGainMeters: request.ElevationGainMeters,
            caloriesBurned: request.CaloriesBurned,
            averagePace: request.AveragePace,
            averageHeartRate: request.AverageHeartRate,
            isFromStrava: false
        );

        await activityRepository.AddAsync(activity, cancellationToken);

        return activity.Id;
    }
}
