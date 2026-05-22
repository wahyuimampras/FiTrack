using FiTrack.Domain.Entities;
using FiTrack.Domain.Interfaces.Services;

namespace FiTrack.Infrastructure.Services;

public class StravaService : IStravaService
{
    public Task<StravaToken> ExchangeCodeAsync(Guid userId, string code, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<string> GetValidAccessTokenAsync(Guid userId, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<List<Activity>> SyncActivitiesAsync(Guid userId, int page = 1, int perPage = 30, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }
}