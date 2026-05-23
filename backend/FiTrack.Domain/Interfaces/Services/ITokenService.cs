using System;
using System.Security.Claims;
using FiTrack.Domain.Entities;

namespace FiTrack.Domain.Interfaces.Services;

public interface ITokenService
{
    string GenerateAccessToken(User user, Guid? sessionId = null);
    string GenerateRefreshToken();
    ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
}