using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FiTrack.Application.DTOs.Common;
using FiTrack.Application.DTOs.Finance;
using FiTrack.Application.Interfaces;
using FiTrack.Domain.Interfaces.Repositories;
using MediatR;

namespace FiTrack.Application.Features.Finance.Queries.GetTransactions;

public class GetTransactionsHandler(
    ITransactionRepository transactionRepository,
    ICurrentUserService currentUserService,
    IMapper mapper) : IRequestHandler<GetTransactionsQuery, PagedResult<TransactionDto>>
{
    public async Task<PagedResult<TransactionDto>> Handle(GetTransactionsQuery request, CancellationToken cancellationToken)
    {
        var userId = currentUserService.UserId;
        if (userId == Guid.Empty) throw new UnauthorizedAccessException("User is not authenticated.");

        FiTrack.Domain.Enums.TransactionType? txType = null;
        if (!string.IsNullOrEmpty(request.Type) && request.Type != "All")
        {
            if (Enum.TryParse<FiTrack.Domain.Enums.TransactionType>(request.Type, true, out var parsed))
            {
                txType = parsed;
            }
        }

        // Ambil data Paged dari Repository
        var (items, totalCount) = await transactionRepository.GetPagedUserTransactionsAsync(
            userId, request.Page, request.PageSize, txType, cancellationToken);

        // Map ke DTO
        var dtos = mapper.Map<IEnumerable<TransactionDto>>(items);

        // Bungkus dengan PagedResult
        return new PagedResult<TransactionDto>(dtos, totalCount, request.Page, request.PageSize);
    }
}