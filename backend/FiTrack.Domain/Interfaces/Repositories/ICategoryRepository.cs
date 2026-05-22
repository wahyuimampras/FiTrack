using FiTrack.Domain.Entities;

namespace FiTrack.Domain.Interfaces.Repositories;

public interface ICategoryRepository
{
    Task<Category?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<IEnumerable<Category>> GetUserCategoriesAsync(Guid userId, CancellationToken ct = default);
    Task AddAsync(Category category, CancellationToken ct = default);
    void Update(Category category);
    void Delete(Category category);
    Task SaveChangesAsync(CancellationToken ct = default);
}