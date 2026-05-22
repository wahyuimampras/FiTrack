using MediatR;
using FiTrack.Application.Features.Workout.Commands.SyncStravaActivities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FiTrack.API.Controllers;

[ApiController]
[Route("api/workouts")]
[Authorize]
public class WorkoutController(ISender mediator) : ControllerBase
{
    [HttpPost("sync-strava")]
    public async Task<IActionResult> SyncStrava()
    {
        var result = await mediator.Send(new SyncStravaActivitiesCommand());
        return Ok(result);
    }
}