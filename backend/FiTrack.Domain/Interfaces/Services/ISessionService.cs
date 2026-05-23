using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FiTrack.Domain.Entities;

namespace FiTrack.Domain.Interfaces.Services;

public interface ISessionService
{
    Task<UserSession> CreateSessionAsync(Guid userId, string refreshToken, string deviceInfo, string ip, CancellationToken ct = default);
    Task<UserSession?> ValidateRefreshTokenAsync(string refreshToken, CancellationToken ct = default);
    Task RevokeSessionAsync(Guid sessionId, CancellationToken ct = default);
    Task RevokeAllSessionsAsync(Guid userId, CancellationToken ct = default);
    Task<bool> IsSessionActiveAsync(Guid sessionId, CancellationToken ct = default);
    Task RotateRefreshTokenAsync(Guid sessionId, string newRefreshToken, CancellationToken ct = default);
    
    // Pastikan ini menggunakan List
    Task<List<UserSession>> GetActiveSessionsAsync(Guid userId, CancellationToken ct = default);
}