using System;
using System.Threading;
using System.Threading.Tasks;
using FiTrack.Application.Interfaces;
using FiTrack.Domain.Interfaces.Services;
using MediatR;

namespace FiTrack.Application.Features.Auth.Commands.RevokeSession;

public class RevokeSessionCommandHandler(
    ISessionService sessionService,
    ICurrentUserService currentUserService) : IRequestHandler<RevokeSessionCommand, Unit>
{
    public async Task<Unit> Handle(RevokeSessionCommand request, CancellationToken cancellationToken)
    {
        var userId = currentUserService.UserId;
        if (userId == Guid.Empty)
        {
            throw new UnauthorizedAccessException("User is not authenticated.");
        }

        // Karena ID sesi berupa GUID yang unik dan sulit ditebak,
        // memanggil fungsi ini langsung cukup aman. 
        // Logic di service idealnya memastikan sesi ini belum di-revoke sebelumnya.
        await sessionService.RevokeSessionAsync(request.SessionId, cancellationToken);

        return Unit.Value;
    }
}