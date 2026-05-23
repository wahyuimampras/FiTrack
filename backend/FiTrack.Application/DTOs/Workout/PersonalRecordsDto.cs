using System.Collections.Generic;

namespace FiTrack.Application.DTOs.Workout;

public class PersonalRecordsDto
{
    public float LongestDistanceMeters { get; set; }
    public float FastestPace { get; set; }
    public int LongestDurationSeconds { get; set; }
    public float HighestElevationGainMeters { get; set; }
}
