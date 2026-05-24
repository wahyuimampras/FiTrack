using FiTrack.Application.DTOs.Common;
using FiTrack.Application.DTOs.Finance;
using MediatR;

namespace FiTrack.Application.Features.Finance.Queries.GetTransactions;

// Default ke Page 1, PageSize 10 jika tidak dikirim dari Frontend
public record GetTransactionsQuery(int Page = 1, int PageSize = 10) : IRequest<PagedResult<TransactionDto>>;