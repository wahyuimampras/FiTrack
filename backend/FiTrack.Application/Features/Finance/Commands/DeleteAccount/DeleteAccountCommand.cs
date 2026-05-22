using System;
using MediatR;

namespace FiTrack.Application.Features.Finance.Commands.DeleteAccount;

public record DeleteAccountCommand(Guid Id) : IRequest<Unit>;