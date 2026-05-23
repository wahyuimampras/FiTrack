using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using FiTrack.Application.Features.Finance.Commands.CreateBudget;
using FiTrack.Application.Features.Finance.Commands.UpdateBudget;
using FiTrack.Application.Features.Finance.Commands.DeleteBudget;
using FiTrack.Application.Features.Finance.Queries.GetBudgets;
using FiTrack.Application.Features.Finance.Queries.GetBudgetById;

namespace FiTrack.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class BudgetController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] short? month, [FromQuery] short? year, CancellationToken cancellationToken)
    {
        var query = new GetBudgetsQuery(month, year);
        var result = await mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetBudgetByIdQuery(id);
        var result = await mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateBudgetCommand command, CancellationToken cancellationToken)
    {
        var id = await mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetAll), new { id }, new { Id = id });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateBudgetCommand command, CancellationToken cancellationToken)
    {
        if (id != command.Id)
        {
            return BadRequest("ID pada URL tidak cocok dengan ID pada body request.");
        }

        await mediator.Send(command, cancellationToken);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var command = new DeleteBudgetCommand(id);
        await mediator.Send(command, cancellationToken);
        return NoContent();
    }
}