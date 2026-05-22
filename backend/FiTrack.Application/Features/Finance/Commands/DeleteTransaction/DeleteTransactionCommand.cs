using MediatR;

namespace FiTrack.Application.Features.Finance.Commands.DeleteTransaction;

public record DeleteTransactionCommand(Guid Id) : IRequest;
