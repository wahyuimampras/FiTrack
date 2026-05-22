using FiTrack.Domain.Interfaces.Repositories;
using FiTrack.Domain.Interfaces.Services;
using FiTrack.Application.Interfaces;
using FiTrack.Infrastructure.Persistence;
using FiTrack.Infrastructure.Persistence.Repositories;
using FiTrack.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FiTrack.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<ITransactionRepository, TransactionRepository>();
        services.AddScoped<IActivityRepository, ActivityRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();

        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<ISessionService, SessionService>();
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddScoped<IStravaService, StravaService>();
        services.AddScoped<IAccountRepository, AccountRepository>();

        return services;
    }
}