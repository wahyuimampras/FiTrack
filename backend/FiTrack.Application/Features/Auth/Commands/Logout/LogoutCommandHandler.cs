using System.Threading;
using System.Threading.Tasks;
using FiTrack.Domain.Interfaces.Services;
using MediatR;

namespace FiTrack.Application.Features.Auth.Commands.Logout;

public class LogoutCommandHandler(ISessionService sessionService) : IRequestHandler<LogoutCommand, Unit>
{
    public async Task<Unit> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        // Cari sesi berdasarkan refresh token yang dikirimkan
        var session = await sessionService.ValidateRefreshTokenAsync(request.RefreshToken, cancellationToken);
        
        if (session != null)
        {
            // Cabut sesi tersebut agar tidak bisa digunakan lagi
            await sessionService.RevokeSessionAsync(session.Id, cancellationToken);
        }

        return Unit.Value;
    }
}