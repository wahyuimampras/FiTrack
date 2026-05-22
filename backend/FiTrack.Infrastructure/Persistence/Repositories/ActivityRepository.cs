using FiTrack.Domain.Entities;
using FiTrack.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FiTrack.Infrastructure.Persistence.Repositories;

public class ActivityRepository(AppDbContext dbContext) : IActivityRepository
{
    public async Task<List<Activity>> GetByDateRangeAsync(Guid userId, DateTime startDate, DateTime endDate, CancellationToken ct = default)
    {
        return await dbContext.Activities
            .Where(a => a.UserId == userId && a.StartDate >= startDate && a.StartDate <= endDate)
            .ToListAsync(ct);
    }
}