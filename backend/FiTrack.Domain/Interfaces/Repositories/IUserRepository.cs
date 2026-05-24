using System;
using System.Threading;
using System.Threading.Tasks;
using FiTrack.Domain.Entities;

namespace FiTrack.Domain.Interfaces.Repositories;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    
    // TAMBAHKAN BARIS INI
    Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default);
    
    Task AddAsync(User user, CancellationToken cancellationToken = default);
    Task UpdateAsync(User user, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}