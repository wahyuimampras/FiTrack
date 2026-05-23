using MediatR;
using FiTrack.Application.Features.Workout.Commands.SyncStravaActivities;
using FiTrack.Application.Features.Workout.Commands.CreateActivity;
using FiTrack.Application.Features.Workout.Queries;
using FiTrack.Application.Features.Workout.Queries.GetPersonalRecords;
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

    [HttpGet]
    public async Task<IActionResult> GetActivities()
    {
        var result = await mediator.Send(new GetActivitiesQuery());
        return Ok(result);
    }

    [HttpGet("statistics")]
    public async Task<IActionResult> GetStatistics([FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
    {
        var query = new GetWorkoutStatisticsQuery 
        { 
            StartDate = startDate, 
            EndDate = endDate 
        };
        var result = await mediator.Send(query);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateActivity([FromBody] CreateActivityCommand command)
    {
        var result = await mediator.Send(command);
        return CreatedAtAction(nameof(GetActivities), new { id = result }, result);
    }

    [HttpGet("personal-records")]
    public async Task<IActionResult> GetPersonalRecords()
    {
        var result = await mediator.Send(new GetPersonalRecordsQuery());
        return Ok(result);
    }
}