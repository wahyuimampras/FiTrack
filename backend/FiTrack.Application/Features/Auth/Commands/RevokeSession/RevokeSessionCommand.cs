using System;
using MediatR;

namespace FiTrack.Application.Features.Auth.Commands.RevokeSession;

public record RevokeSessionCommand(Guid SessionId) : IRequest<Unit>;