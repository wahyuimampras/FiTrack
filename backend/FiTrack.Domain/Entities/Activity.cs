using System;
using FiTrack.Domain.Enums;

namespace FiTrack.Domain.Entities;

public class Activity
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public long? StravaActivityId { get; private set; }
    public ActivityType Type { get; private set; }
    public string Name { get; private set; }
    public float DistanceMeters { get; private set; }
    public int DurationSeconds { get; private set; }
    public float? ElevationGainMeters { get; private set; }
    public int? CaloriesBurned { get; private set; }
    public float? AveragePace { get; private set; }
    public float? AverageHeartRate { get; private set; }
    public DateTime StartDate { get; private set; }
    public bool IsFromStrava { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public User User { get; private set; }

    private Activity() { }

    public static Activity Create(
        Guid userId, ActivityType type, string name, 
        float distanceMeters, int durationSeconds, DateTime startDate,
        long? stravaActivityId = null, float? elevationGainMeters = null, 
        int? caloriesBurned = null, float? averagePace = null, 
        float? averageHeartRate = null, bool isFromStrava = false)
    {
        return new Activity
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            StravaActivityId = stravaActivityId,
            Type = type,
            Name = name,
            DistanceMeters = distanceMeters,
            DurationSeconds = durationSeconds,
            ElevationGainMeters = elevationGainMeters,
            CaloriesBurned = caloriesBurned,
            AveragePace = averagePace,
            AverageHeartRate = averageHeartRate,
            StartDate = startDate,
            IsFromStrava = isFromStrava,
            CreatedAt = DateTime.UtcNow
        };
    }
}
