using MediatR;
using FiTrack.Application.DTOs.Finance;

namespace FiTrack.Application.Features.Finance.Queries.GetDashboardSummary;

public record GetDashboardSummaryQuery(int Month, int Year) : IRequest<DashboardSummaryDto>;