using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FiTrack.Application.DTOs.Auth;
using FiTrack.Application.Interfaces;
using FiTrack.Domain.Interfaces.Services;
using MediatR;

namespace FiTrack.Application.Features.Auth.Queries.GetSessions;

public class GetSessionsHandler(
    ISessionService sessionService,
    ICurrentUserService currentUserService,
    IMapper mapper) : IRequestHandler<GetSessionsQuery, IEnumerable<SessionDto>>
{
    public async Task<IEnumerable<SessionDto>> Handle(GetSessionsQuery request, CancellationToken cancellationToken)
    {
        var userId = currentUserService.UserId;
        if (userId == Guid.Empty)
        {
            throw new UnauthorizedAccessException("User is not authenticated.");
        }

        // Catatan: Pastikan ISessionService Anda memiliki method ini, atau sesuaikan namanya
        // misal: GetAllByUserIdAsync atau GetUserSessionsAsync
        var sessions = await sessionService.GetActiveSessionsAsync(userId, cancellationToken);

        return mapper.Map<IEnumerable<SessionDto>>(sessions);
    }
}