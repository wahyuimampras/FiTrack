using System;
using MediatR;
using FiTrack.Domain.Enums;

namespace FiTrack.Application.Features.Workout.Commands.CreateActivity;

public class CreateActivityCommand : IRequest<Guid>
{
    public ActivityType Type { get; set; }
    public string Name { get; set; } = string.Empty;
    public float DistanceMeters { get; set; }
    public int DurationSeconds { get; set; }
    public float? ElevationGainMeters { get; set; }
    public int? CaloriesBurned { get; set; }
    public float? AveragePace { get; set; }
    public float? AverageHeartRate { get; set; }
    public DateTime StartDate { get; set; }
}
