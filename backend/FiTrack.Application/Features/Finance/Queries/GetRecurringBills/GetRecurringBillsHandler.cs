using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FiTrack.Application.DTOs.Finance;
using FiTrack.Application.Interfaces;
using FiTrack.Domain.Interfaces.Repositories;
using MediatR;

namespace FiTrack.Application.Features.Finance.Queries.GetRecurringBills;

public class GetRecurringBillsHandler(
    IRecurringBillRepository recurringBillRepository,
    ICurrentUserService currentUserService,
    IMapper mapper) : IRequestHandler<GetRecurringBillsQuery, IEnumerable<RecurringBillDto>>
{
    public async Task<IEnumerable<RecurringBillDto>> Handle(GetRecurringBillsQuery request, CancellationToken cancellationToken)
    {
        var userId = currentUserService.UserId;
        if (userId == Guid.Empty)
        {
            throw new UnauthorizedAccessException("User is not authenticated.");
        }

        var bills = await recurringBillRepository.GetByUserIdAsync(userId, cancellationToken);

        return mapper.Map<IEnumerable<RecurringBillDto>>(bills);
    }
}