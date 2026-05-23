using System.Net.Http.Json;
using System.Text.Json.Serialization;
using FiTrack.Domain.Entities;
using FiTrack.Domain.Exceptions;
using FiTrack.Domain.Interfaces.Services;
using FiTrack.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace FiTrack.Infrastructure.Services;

public class StravaTokenResponse
{
    [JsonPropertyName("access_token")]
    public string AccessToken { get; set; } = string.Empty;

    [JsonPropertyName("refresh_token")]
    public string RefreshToken { get; set; } = string.Empty;

    [JsonPropertyName("expires_at")]
    public long ExpiresAt { get; set; }

    [JsonPropertyName("athlete")]
    public StravaAthlete Athlete { get; set; } = new();
}

public class StravaAthlete
{
    [JsonPropertyName("id")]
    public long Id { get; set; }
}

public class StravaActivityDto
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    [JsonPropertyName("distance")]
    public float Distance { get; set; }

    [JsonPropertyName("moving_time")]
    public int MovingTime { get; set; }

    [JsonPropertyName("total_elevation_gain")]
    public float TotalElevationGain { get; set; }

    [JsonPropertyName("calories")]
    public int Calories { get; set; }

    [JsonPropertyName("start_date")]
    public DateTime StartDate { get; set; }
}

public class StravaService(
    HttpClient httpClient,
    AppDbContext db,
    IConfiguration config
) : IStravaService
{
    private readonly string _clientId = config["Strava:ClientId"] ?? "";
    private readonly string _clientSecret = config["Strava:ClientSecret"] ?? "";

    public string GetAuthUrl(string redirectUri)
    {
        return $"https://www.strava.com/oauth/authorize?client_id={_clientId}&response_type=code&redirect_uri={redirectUri}&scope=read,activity:read_all";
    }

    public async Task<StravaToken> ExchangeCodeAsync(Guid userId, string code, CancellationToken ct = default)
    {
        var response = await httpClient.PostAsJsonAsync(
            "https://www.strava.com/oauth/token",
            new
            {
                client_id = _clientId,
                client_secret = _clientSecret,
                code,
                grant_type = "authorization_code"
            }, ct);

        response.EnsureSuccessStatusCode();
        var data = await response.Content.ReadFromJsonAsync<StravaTokenResponse>(cancellationToken: ct);
        
        if (data == null) throw new DomainException("Failed to exchange token from Strava.");

        // Clean up old token if exists
        var existingToken = await db.StravaTokens.FirstOrDefaultAsync(t => t.UserId == userId, ct);
        if (existingToken != null)
        {
            db.StravaTokens.Remove(existingToken);
        }

        var token = StravaToken.Create(
            userId,
            data.AccessToken,
            data.RefreshToken,
            DateTimeOffset.FromUnixTimeSeconds(data.ExpiresAt).UtcDateTime,
            data.Athlete.Id.ToString()
        );

        var user = await db.Users.FindAsync(new object[] { userId }, ct);
        if (user != null)
        {
            user.ConnectStrava(data.Athlete.Id.ToString());
        }

        db.StravaTokens.Add(token);
        await db.SaveChangesAsync(ct);
        return token;
    }

    public async Task<string> GetValidAccessTokenAsync(Guid userId, CancellationToken ct = default)
    {
        var token = await db.StravaTokens
            .FirstOrDefaultAsync(t => t.UserId == userId, ct);
            
        if (token == null)
        {
            throw new DomainException("Strava not connected");
        }

        if (!token.IsExpired) return token.AccessToken;

        // Auto-refresh token
        var response = await httpClient.PostAsJsonAsync(
            "https://www.strava.com/oauth/token",
            new
            {
                client_id = _clientId,
                client_secret = _clientSecret,
                refresh_token = token.RefreshToken,
                grant_type = "refresh_token"
            }, ct);

        response.EnsureSuccessStatusCode();
        var data = await response.Content.ReadFromJsonAsync<StravaTokenResponse>(cancellationToken: ct);
        
        if (data == null) throw new DomainException("Failed to refresh token from Strava.");

        token.Refresh(
            data.AccessToken,
            data.RefreshToken,
            DateTimeOffset.FromUnixTimeSeconds(data.ExpiresAt).UtcDateTime
        );

        await db.SaveChangesAsync(ct);
        return token.AccessToken;
    }

    public async Task<List<Activity>> SyncActivitiesAsync(Guid userId, int page = 1, int perPage = 30, CancellationToken ct = default)
    {
        var accessToken = await GetValidAccessTokenAsync(userId, ct);

        httpClient.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

        var stravaActivities = await httpClient.GetFromJsonAsync<List<StravaActivityDto>>(
            $"https://www.strava.com/api/v3/athlete/activities?page={page}&per_page={perPage}", ct);

        var activities = new List<Activity>();
        foreach (var dto in stravaActivities ?? [])
        {
            var exists = await db.Activities
                .AnyAsync(a => a.StravaActivityId == dto.Id, ct);

            if (!exists)
            {
                // Parse activity type safely
                if (Enum.TryParse<FiTrack.Domain.Enums.ActivityType>(dto.Type, true, out var type))
                {
                    var activity = Activity.Create(
                        userId,
                        type,
                        dto.Name,
                        dto.Distance,
                        dto.MovingTime,
                        dto.StartDate,
                        stravaActivityId: dto.Id,
                        elevationGainMeters: dto.TotalElevationGain,
                        caloriesBurned: dto.Calories,
                        averagePace: dto.Distance > 0 ? dto.MovingTime / (dto.Distance / 1000f) : null,
                        isFromStrava: true
                    );
                    db.Activities.Add(activity);
                    activities.Add(activity);
                }
            }
        }

        await db.SaveChangesAsync(ct);
        return activities;
    }

    public async Task DisconnectAsync(Guid userId, CancellationToken ct = default)
    {
        var token = await db.StravaTokens.FirstOrDefaultAsync(t => t.UserId == userId, ct);
        if (token != null)
        {
            db.StravaTokens.Remove(token);
            
            var user = await db.Users.FindAsync(new object[] { userId }, ct);
            if (user != null)
            {
                user.DisconnectStrava();
            }
            
            await db.SaveChangesAsync(ct);
        }
    }
}
