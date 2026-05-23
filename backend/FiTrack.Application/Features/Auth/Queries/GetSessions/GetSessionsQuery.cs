using System.Collections.Generic;
using FiTrack.Application.DTOs.Auth;
using MediatR;

namespace FiTrack.Application.Features.Auth.Queries.GetSessions;

public record GetSessionsQuery() : IRequest<IEnumerable<SessionDto>>;