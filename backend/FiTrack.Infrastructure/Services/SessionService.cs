using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FiTrack.Domain.Entities;
using FiTrack.Domain.Interfaces.Services;
using FiTrack.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace FiTrack.Infrastructure.Services;

public class SessionService(AppDbContext db) : ISessionService
{
    public async Task<UserSession> CreateSessionAsync(
        Guid userId, string refreshToken,
        string deviceInfo, string ip,
        CancellationToken ct = default)
    {
        var existingSessions = await db.UserSessions
            .Where(s => s.UserId == userId && s.RevokedAt == null && s.ExpiresAt > DateTime.UtcNow)
            .OrderBy(s => s.CreatedAt)
            .ToListAsync(ct);

        if (existingSessions.Count >= 5)
        {
            existingSessions.First().Revoke();
        }

        var hash = BCrypt.Net.BCrypt.HashPassword(refreshToken);
        var session = UserSession.Create(userId, refreshToken, hash, deviceInfo, ip);
        
        db.UserSessions.Add(session);
        await db.SaveChangesAsync(ct);
        
        return session;
    }

    public async Task<UserSession?> ValidateRefreshTokenAsync(string refreshToken, CancellationToken ct = default)
    {
        var sessions = await db.UserSessions
            .Include(s => s.User)
            .Where(s => s.RevokedAt == null && s.ExpiresAt > DateTime.UtcNow)
            .ToListAsync(ct);

        return sessions.FirstOrDefault(s => BCrypt.Net.BCrypt.Verify(refreshToken, s.RefreshTokenHash));
    }

    public async Task RevokeSessionAsync(Guid sessionId, CancellationToken ct = default)
    {
        // FIX: Gunakan new object[] agar tidak error bentrok dengan CancellationToken
        var session = await db.UserSessions.FindAsync(new object[] { sessionId }, ct);
        if (session != null && session.RevokedAt == null)
        {
            session.Revoke();
            await db.SaveChangesAsync(ct);
        }
    }

    public async Task RevokeAllSessionsAsync(Guid userId, CancellationToken ct = default)
    {
        var sessions = await db.UserSessions
            .Where(s => s.UserId == userId && s.RevokedAt == null)
            .ToListAsync(ct);

        if (sessions.Any())
        {
            sessions.ForEach(s => s.Revoke());
            await db.SaveChangesAsync(ct);
        }
    }

    public async Task<bool> IsSessionActiveAsync(Guid sessionId, CancellationToken ct = default)
    {
        // FIX: Sama seperti di atas
        var session = await db.UserSessions.FindAsync(new object[] { sessionId }, ct);
        return session != null && session.IsActive;
    }

// Lokasi: backend/FiTrack.Infrastructure/Services/SessionService.cs

    public async Task RotateRefreshTokenAsync(Guid sessionId, string newRefreshToken, CancellationToken ct = default)
    {
        var session = await db.UserSessions.FindAsync(new object[] { sessionId }, ct);
        
        if (session != null)
        {
            // Generate hash untuk token baru
            var hash = BCrypt.Net.BCrypt.HashPassword(newRefreshToken);
            
            // Panggil method dengan 2 argumen: plain text dan hash
            session.RotateToken(newRefreshToken, hash);
            
            await db.SaveChangesAsync(ct);
        }
    }

    // FIX: Implementasi method sesuai dengan Interface bawaan Anda (GetActiveSessionsAsync)
    public async Task<List<UserSession>> GetActiveSessionsAsync(Guid userId, CancellationToken ct = default)
    {
        return await db.UserSessions
            .Where(s => s.UserId == userId && s.RevokedAt == null && s.ExpiresAt > DateTime.UtcNow)
            .OrderByDescending(s => s.CreatedAt)
            .ToListAsync(ct);
    }
}