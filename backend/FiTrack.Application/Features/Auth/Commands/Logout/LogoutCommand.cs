using MediatR;

namespace FiTrack.Application.Features.Auth.Commands.Logout;

public record LogoutCommand(string RefreshToken) : IRequest<Unit>;