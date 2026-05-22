using System;

namespace FiTrack.Domain.Entities;

public class StravaToken
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public string AccessToken { get; private set; }
    public string RefreshToken { get; private set; }
    public DateTime ExpiresAt { get; private set; }
    public string AthleteId { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    public User User { get; private set; }

    public bool IsExpired => DateTime.UtcNow >= ExpiresAt;

    private StravaToken() { }

    public static StravaToken Create(
        Guid userId, string accessToken,
        string refreshToken, DateTime expiresAt, string athleteId)
    {
        return new StravaToken
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            ExpiresAt = expiresAt,
            AthleteId = athleteId,
            UpdatedAt = DateTime.UtcNow
        };
    }

    public void Refresh(string newAccessToken, string newRefreshToken, DateTime newExpiresAt)
    {
        AccessToken = newAccessToken;
        RefreshToken = newRefreshToken;
        ExpiresAt = newExpiresAt;
        UpdatedAt = DateTime.UtcNow;
    }
}
