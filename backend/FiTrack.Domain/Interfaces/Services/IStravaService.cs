using FiTrack.Domain.Entities;

namespace FiTrack.Domain.Interfaces.Services;

public interface IStravaService
{
    Task<StravaToken> ExchangeCodeAsync(Guid userId, string code, CancellationToken ct = default);
    Task<string> GetValidAccessTokenAsync(Guid userId, CancellationToken ct = default);
    Task<List<Activity>> SyncActivitiesAsync(Guid userId, int page = 1, int perPage = 30, CancellationToken ct = default);
}