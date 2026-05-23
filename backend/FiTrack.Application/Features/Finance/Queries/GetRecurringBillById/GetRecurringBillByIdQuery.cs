using System;
using FiTrack.Application.DTOs.Finance;
using MediatR;

namespace FiTrack.Application.Features.Finance.Queries.GetRecurringBillById;

public record GetRecurringBillByIdQuery(Guid Id) : IRequest<RecurringBillDto>;