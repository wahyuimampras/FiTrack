using FiTrack.Domain.Entities;
using FiTrack.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FiTrack.Infrastructure.Persistence.Repositories;

public class UserRepository(AppDbContext dbContext) : IUserRepository
{
    public async Task<User?> GetByEmailAsync(string email, CancellationToken ct = default)
    {
        return await dbContext.Users.FirstOrDefaultAsync(u => u.Email == email, ct);
    }

    public async Task<User?> GetByUsernameAsync(string username, CancellationToken ct = default)
    {
        return await dbContext.Users.FirstOrDefaultAsync(u => u.Username == username, ct);
    }

    public async Task AddAsync(User user, CancellationToken ct = default)
    {
        await dbContext.Users.AddAsync(user, ct);
    }

    public async Task SaveChangesAsync(CancellationToken ct = default)
    {
        await dbContext.SaveChangesAsync(ct);
    }
}