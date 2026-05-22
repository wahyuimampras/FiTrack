using FiTrack.Application.DTOs.Finance;
using MediatR;

namespace FiTrack.Application.Features.Finance.Queries.GetTransactionById;

public record GetTransactionByIdQuery(Guid Id) : IRequest<TransactionDto>;
