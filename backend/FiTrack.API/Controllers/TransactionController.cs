using FiTrack.Application.Features.Finance.Commands.CreateTransaction;
using FiTrack.Application.Features.Finance.Commands.UpdateTransaction;
using FiTrack.Application.Features.Finance.Commands.DeleteTransaction;
using FiTrack.Application.Features.Finance.Queries.GetTransactions;
using FiTrack.Application.Features.Finance.Queries.GetTransactionById;
using FiTrack.Application.Features.Finance.Queries.GetTransactionSummary;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FiTrack.API.Controllers;

[ApiController]
[Route("api/transactions")]
[Authorize]
public class TransactionController(ISender mediator) : ControllerBase
{
    [HttpGet("summary")]
    public async Task<IActionResult> GetSummary(CancellationToken cancellationToken = default)
    {
        var result = await mediator.Send(new GetTransactionSummaryQuery(), cancellationToken);
        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string? type = null, CancellationToken cancellationToken = default)
    {
        // Pastikan angka tidak minus atau 0
        if (page < 1) page = 1;
        if (pageSize < 1) pageSize = 10;
        if (pageSize > 100) pageSize = 100; // Batas maksimal penarikan data agar tidak dijebol

        var query = new GetTransactionsQuery(page, pageSize, type);
        var result = await mediator.Send(query, cancellationToken);
        
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetTransactionById(Guid id)
    {
        var result = await mediator.Send(new GetTransactionByIdQuery(id));
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateTransaction([FromBody] CreateTransactionCommand command)
    {
        var result = await mediator.Send(command);
        return Ok(result);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateTransaction(Guid id, [FromBody] UpdateTransactionCommand command)
    {
        if (id != command.Id) return BadRequest();
        var result = await mediator.Send(command);
        return Ok(result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteTransaction(Guid id)
    {
        await mediator.Send(new DeleteTransactionCommand(id));
        return NoContent();
    }
}
