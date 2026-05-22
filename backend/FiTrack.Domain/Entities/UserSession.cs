using System;

namespace FiTrack.Domain.Entities;

public class UserSession
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public string RefreshToken { get; private set; }
    public string RefreshTokenHash { get; private set; }
    public string DeviceInfo { get; private set; }
    public string IpAddress { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime ExpiresAt { get; private set; }
    public DateTime? RevokedAt { get; private set; }

    public User User { get; private set; }

    public bool IsRevoked => RevokedAt.HasValue;
    public bool IsExpired => DateTime.UtcNow >= ExpiresAt;
    public bool IsActive => !IsRevoked && !IsExpired;

    private UserSession() { }

    public static UserSession Create(
        Guid userId, string refreshToken, string refreshTokenHash,
        string deviceInfo, string ipAddress)
    {
        return new UserSession
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            RefreshToken = refreshToken,
            RefreshTokenHash = refreshTokenHash,
            DeviceInfo = deviceInfo,
            IpAddress = ipAddress,
            CreatedAt = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow.AddDays(7)
        };
    }

    public void Revoke() => RevokedAt = DateTime.UtcNow;

    public void RotateToken(string newRefreshTokenHash)
    {
        RefreshTokenHash = newRefreshTokenHash;
    }
}
