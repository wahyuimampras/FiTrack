using System;
using System.Collections.Generic;
using MediatR;
using FiTrack.Application.DTOs.Auth;

namespace FiTrack.Application.Features.Auth.Queries.GetSessions;

public record GetSessionsQuery : IRequest<List<SessionDto>>;
