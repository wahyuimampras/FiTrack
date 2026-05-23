using MediatR;
using FiTrack.Application.DTOs.Finance;

namespace FiTrack.Application.Features.Finance.Queries.GetMonthlyReport;

public record GetMonthlyReportQuery(int Month, int Year) : IRequest<MonthlyReportDto>;
