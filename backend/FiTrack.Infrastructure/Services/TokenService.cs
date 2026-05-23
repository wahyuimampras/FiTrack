using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using FiTrack.Domain.Entities;
using FiTrack.Domain.Interfaces.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace FiTrack.Infrastructure.Services;

public class TokenService(IConfiguration config) : ITokenService
{
    private readonly string _secret = config["Jwt:Secret"] ?? "GANTI_DENGAN_SECRET_MINIMAL_32_KARAKTER_RANDOM";
    private readonly string _issuer = config["Jwt:Issuer"] ?? "FiTrack";
    private readonly string _audience = config["Jwt:Audience"] ?? "FiTrack-Users";

    public string GenerateAccessToken(User user, Guid? sessionId = null)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Email, user.Email),
            new(ClaimTypes.Name, user.Username),
            new("stravaConnected", (user.StravaAthleteId != null).ToString()),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        if (sessionId.HasValue)
        {
            claims.Add(new Claim("sessionId", sessionId.Value.ToString()));
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _issuer,
            audience: _audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(15),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public string GenerateRefreshToken()
    {
        var bytes = new byte[64];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(bytes);
        return Convert.ToBase64String(bytes);
    }

    public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = false, // PENTING: Abaikan pengecekan expired
                ValidateIssuerSigningKey = true,
                ValidIssuer = _issuer,
                ValidAudience = _audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secret))
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);

            if (securityToken is not JwtSecurityToken jwtSecurityToken || 
                !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new UnauthorizedAccessException("Token tidak valid.");
            }

            return principal;
        }
}