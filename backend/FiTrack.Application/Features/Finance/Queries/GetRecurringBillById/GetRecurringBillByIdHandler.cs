using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FiTrack.Application.DTOs.Finance;
using FiTrack.Application.Interfaces;
using FiTrack.Domain.Interfaces.Repositories;
using MediatR;

namespace FiTrack.Application.Features.Finance.Queries.GetRecurringBillById;

public class GetRecurringBillByIdHandler(
    IRecurringBillRepository recurringBillRepository,
    ICurrentUserService currentUserService,
    IMapper mapper) : IRequestHandler<GetRecurringBillByIdQuery, RecurringBillDto>
{
    public async Task<RecurringBillDto> Handle(GetRecurringBillByIdQuery request, CancellationToken cancellationToken)
    {
        var userId = currentUserService.UserId;
        if (userId == Guid.Empty)
        {
            throw new UnauthorizedAccessException("User is not authenticated.");
        }

        var bill = await recurringBillRepository.GetByIdAsync(request.Id, userId, cancellationToken);
        if (bill == null)
        {
            throw new KeyNotFoundException("Recurring bill not found.");
        }

        return mapper.Map<RecurringBillDto>(bill);
    }
}