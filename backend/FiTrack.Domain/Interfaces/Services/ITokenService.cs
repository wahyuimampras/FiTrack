using System.Security.Claims;
using FiTrack.Domain.Entities;

namespace FiTrack.Domain.Interfaces.Services;

public interface ITokenService
{
    string GenerateAccessToken(User user);
    string GenerateRefreshToken();
    ClaimsPrincipal? ValidateToken(string token);
}