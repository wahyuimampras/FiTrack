using FiTrack.Application.DTOs.Auth;
using MediatR;

namespace FiTrack.Application.Features.Auth.Commands.RefreshToken;

public record RefreshTokenCommand(
    string AccessToken,
    string RefreshToken
) : IRequest<TokenResponseDto>;