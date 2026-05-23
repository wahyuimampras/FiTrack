using FiTrack.Domain.Entities;

namespace FiTrack.Domain.Interfaces.Repositories;

public interface IActivityRepository
{
    Task<List<Activity>> GetByDateRangeAsync(Guid userId, DateTime startDate, DateTime endDate, CancellationToken ct = default);
    Task<List<Activity>> GetByUserIdAsync(Guid userId, CancellationToken ct = default);
    Task AddAsync(Activity activity, CancellationToken ct = default);
}