namespace FiTrack.Application.DTOs.Workout;

public class WorkoutStatisticsDto
{
    public int TotalActivities { get; set; }
    public float TotalDistanceMeters { get; set; }
    public int TotalDurationSeconds { get; set; }
    public int TotalCaloriesBurned { get; set; }
    public float TotalElevationGainMeters { get; set; }
}
