namespace FiTrack.Application.DTOs.Auth;

public record TokenResponseDto(
    string AccessToken,
    string RefreshToken,
    DateTime ExpiryTime);