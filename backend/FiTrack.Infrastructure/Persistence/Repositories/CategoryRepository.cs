using FiTrack.Domain.Entities;
using FiTrack.Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace FiTrack.Infrastructure.Persistence.Repositories;

public class CategoryRepository(AppDbContext context) : ICategoryRepository
{
    public async Task<Category?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await context.Categories.FirstOrDefaultAsync(c => c.Id == id, ct);
    }

    public async Task<IEnumerable<Category>> GetUserCategoriesAsync(Guid userId, CancellationToken ct = default)
    {
        return await context.Categories
            .Where(c => c.UserId == userId || c.IsDefault)
            .ToListAsync(ct);
    }

    public async Task AddAsync(Category category, CancellationToken ct = default)
    {
        await context.Categories.AddAsync(category, ct);
    }

    public void Update(Category category)
    {
        context.Categories.Update(category);
    }

    public void Delete(Category category)
    {
        context.Categories.Remove(category);
    }

    public async Task SaveChangesAsync(CancellationToken ct = default)
    {
        await context.SaveChangesAsync(ct);
    }
}