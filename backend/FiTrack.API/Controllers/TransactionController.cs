using FiTrack.Application.Features.Finance.Commands.CreateTransaction;
using FiTrack.Application.Features.Finance.Commands.UpdateTransaction;
using FiTrack.Application.Features.Finance.Commands.DeleteTransaction;
using FiTrack.Application.Features.Finance.Queries.GetTransactions;
using FiTrack.Application.Features.Finance.Queries.GetTransactionById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FiTrack.API.Controllers;

[ApiController]
[Route("api/transactions")]
[Authorize]
public class TransactionController(ISender mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetTransactions()
    {
        var result = await mediator.Send(new GetTransactionsQuery());
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
