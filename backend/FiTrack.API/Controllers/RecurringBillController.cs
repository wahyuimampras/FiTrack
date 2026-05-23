using System;
using System.Threading;
using System.Threading.Tasks;
using FiTrack.Application.Features.Finance.Commands.CreateRecurringBill;
using FiTrack.Application.Features.Finance.Commands.DeleteRecurringBill;
using FiTrack.Application.Features.Finance.Commands.UpdateRecurringBill;
using FiTrack.Application.Features.Finance.Queries.GetRecurringBills;
using FiTrack.Application.Features.Finance.Queries.GetRecurringBillById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FiTrack.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class RecurringBillController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetRecurringBillsQuery(), cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetRecurringBillByIdQuery(id);
        var result = await mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateRecurringBillCommand command, CancellationToken cancellationToken)
    {
        var id = await mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id }, new { Id = id });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateRecurringBillCommand command, CancellationToken cancellationToken)
    {
        if (id != command.Id)
        {
            return BadRequest("ID mismatch");
        }

        await mediator.Send(command, cancellationToken);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await mediator.Send(new DeleteRecurringBillCommand(id), cancellationToken);
        return NoContent();
    }
}