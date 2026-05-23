using System.Threading;
using System.Threading.Tasks;
using FiTrack.Application.DTOs.Workout;
using FiTrack.Application.Features.Workout.Commands.DisconnectStrava;
using FiTrack.Application.Features.Workout.Commands.ExchangeStravaToken;
using MediatR;
using FiTrack.Application.Features.Workout.Commands.SyncStravaActivities;
using FiTrack.Application.Features.Workout.Queries.GetStravaAuthUrl;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FiTrack.API.Controllers;

[ApiController]
[Route("api/strava")]
[Authorize]
public class StravaController(ISender mediator) : ControllerBase
{
    [HttpGet("auth-url")]
    public async Task<IActionResult> GetAuthUrl([FromQuery] string redirectUri)
    {
        if (string.IsNullOrEmpty(redirectUri))
        {
            return BadRequest(new { message = "redirectUri is required." });
        }

        // Memanggil Application layer (Query) untuk mendapatkan URL Auth
        var result = await mediator.Send(new GetStravaAuthUrlQuery { RedirectUri = redirectUri });
        return Ok(new { url = result });
    }

    [HttpPost("exchange-token")]
    public async Task<IActionResult> ExchangeToken([FromBody] ExchangeTokenDto request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(request.Code))
        {
            return BadRequest(new { message = "code is required." });
        }

        await mediator.Send(new ExchangeStravaTokenCommand { Code = request.Code }, cancellationToken);
        return Ok(new { message = "Strava connected successfully." });
    }

    [HttpPost("sync")]
    public async Task<IActionResult> Sync(CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new SyncStravaActivitiesCommand(), cancellationToken);
        return Ok(result);
    }

    [HttpDelete("disconnect")]
    public async Task<IActionResult> Disconnect(CancellationToken cancellationToken)
    {
        await mediator.Send(new DisconnectStravaCommand(), cancellationToken);
        return Ok(new { message = "Strava disconnected successfully." });
    }
}