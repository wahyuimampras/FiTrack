using System;
using FiTrack.Application.DTOs.Finance;
using MediatR;

namespace FiTrack.Application.Features.Finance.Queries.GetBudgetById;

public record GetBudgetByIdQuery(Guid Id) : IRequest<BudgetDto>;