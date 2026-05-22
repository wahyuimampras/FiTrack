using MediatR;
using FiTrack.Application.Features.Finance.Queries.GetDashboardSummary;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FiTrack.API.Controllers;

[ApiController]
[Route("api/dashboard")]
[Authorize]
public class DashboardController(ISender mediator) : ControllerBase
{
    [HttpGet("summary")]
    public async Task<IActionResult> GetSummary([FromQuery] int month, [FromQuery] int year)
    {
        var result = await mediator.Send(new GetDashboardSummaryQuery(month, year));
        return Ok(result);
    }
}