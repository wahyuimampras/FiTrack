using System.Collections.Generic;
using FiTrack.Application.DTOs.Finance;
using MediatR;

namespace FiTrack.Application.Features.Finance.Queries.GetRecurringBills;

public record GetRecurringBillsQuery() : IRequest<IEnumerable<RecurringBillDto>>;