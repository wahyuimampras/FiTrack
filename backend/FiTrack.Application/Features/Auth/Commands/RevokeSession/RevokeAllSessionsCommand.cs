using MediatR;

namespace FiTrack.Application.Features.Auth.Commands.RevokeAllSessions;

public record RevokeAllSessionsCommand() : IRequest<Unit>;