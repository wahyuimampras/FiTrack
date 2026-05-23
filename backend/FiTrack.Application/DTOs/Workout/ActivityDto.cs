using System;

namespace FiTrack.Application.DTOs.Workout;

public class ActivityDto
{
    public Guid Id { get; set; }
    public long? StravaActivityId { get; set; }
    public string Type { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public float DistanceMeters { get; set; }
    public int DurationSeconds { get; set; }
    public float? ElevationGainMeters { get; set; }
    public int? CaloriesBurned { get; set; }
    public float? AveragePace { get; set; }
    public float? AverageHeartRate { get; set; }
    public DateTime StartDate { get; set; }
    public bool IsFromStrava { get; set; }
    public DateTime CreatedAt { get; set; }
}
