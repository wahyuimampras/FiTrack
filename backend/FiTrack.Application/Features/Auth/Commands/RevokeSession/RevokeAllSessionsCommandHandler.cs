using System;
using System.Threading;
using System.Threading.Tasks;
using FiTrack.Application.Interfaces;
using FiTrack.Domain.Interfaces.Services;
using MediatR;

namespace FiTrack.Application.Features.Auth.Commands.RevokeAllSessions;

public class RevokeAllSessionsCommandHandler(
    ISessionService sessionService,
    ICurrentUserService currentUserService) : IRequestHandler<RevokeAllSessionsCommand, Unit>
{
    public async Task<Unit> Handle(RevokeAllSessionsCommand request, CancellationToken cancellationToken)
    {
        var userId = currentUserService.UserId;
        if (userId == Guid.Empty)
        {
            throw new UnauthorizedAccessException("User is not authenticated.");
        }

        // Matikan semua sesi aktif milik user yang sedang login
        await sessionService.RevokeAllSessionsAsync(userId, cancellationToken);

        return Unit.Value;
    }
}