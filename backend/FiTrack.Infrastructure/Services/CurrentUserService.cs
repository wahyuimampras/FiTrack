using FiTrack.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace FiTrack.Infrastructure.Services;

public class CurrentUserService(IHttpContextAccessor httpContextAccessor) : ICurrentUserService
{
    public Guid UserId 
    { 
        get 
        {
            var userId = httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            return userId != null ? Guid.Parse(userId) : Guid.Empty;
        }
    }
}