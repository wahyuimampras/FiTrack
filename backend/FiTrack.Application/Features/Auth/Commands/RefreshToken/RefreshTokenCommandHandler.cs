using System;
using System.Security.Claims;
using FiTrack.Application.DTOs.Auth;
using FiTrack.Domain.Entities;
using FiTrack.Domain.Interfaces.Repositories;
using FiTrack.Domain.Interfaces.Services;
using MediatR;

namespace FiTrack.Application.Features.Auth.Commands.RefreshToken;

public class RefreshTokenCommandHandler(
    IUserRepository userRepository,
    ITokenService tokenService,
    ISessionService sessionService) : IRequestHandler<RefreshTokenCommand, TokenResponseDto>
{
    public async Task<TokenResponseDto> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        // 1. Dapatkan Claims Principal dari Access Token yang expired tanpa memvalidasi masa berlakunya
        ClaimsPrincipal principal;
        try
        {
            principal = tokenService.GetPrincipalFromExpiredToken(request.AccessToken);
        }
        catch
        {
            throw new UnauthorizedAccessException("Access token tidak valid.");
        }

        // 2. Ambil Email atau Username dari Claims untuk mencari data User
        var email = principal.FindFirst(ClaimTypes.Email)?.Value 
            ?? principal.FindFirst(ClaimTypes.Name)?.Value;

        if (string.IsNullOrEmpty(email))
        {
            throw new UnauthorizedAccessException("Token tidak mengandung identitas pengguna yang valid.");
        }

        var user = await userRepository.GetByEmailAsync(email, cancellationToken);
        if (user == null)
        {
            throw new UnauthorizedAccessException("Pengguna tidak ditemukan.");
        }

        // 3. Validasi Refresh Token ke session database menggunakan ISessionService
        var session = await sessionService.ValidateRefreshTokenAsync(request.RefreshToken, cancellationToken);
        if (session == null || session.UserId != user.Id)
        {
            throw new UnauthorizedAccessException("Refresh token tidak valid atau telah kedaluwarsa.");
        }
        // 4. Generate pasangan token baru
        var newAccessToken = tokenService.GenerateAccessToken(user);
        var newRefreshToken = tokenService.GenerateRefreshToken();

        // 5. Perbarui session di database menggunakan method bawaan SessionService
        // Revoke (nonaktifkan) session yang lama
        await sessionService.RevokeSessionAsync(session.Id, cancellationToken);
        
        // Buat session baru (ISessionService sudah otomatis melakukan hashing dan SaveChanges)
        await sessionService.CreateSessionAsync(
            user.Id, 
            newRefreshToken, 
            session.DeviceInfo, 
            session.IpAddress, 
            cancellationToken);

        // Eksekusi token rotasi baru:
        var tokenExpiry = DateTime.UtcNow.AddMinutes(15); // Durasi Access Token baru

        return new TokenResponseDto(newAccessToken, newRefreshToken, tokenExpiry);
    }
}