using System;
using System.Threading;
using System.Threading.Tasks;
using FiTrack.Application.Features.Finance.Commands.CreateAccount;
using FiTrack.Application.Features.Finance.Commands.DeleteAccount;
using FiTrack.Application.Features.Finance.Commands.UpdateAccount;
using FiTrack.Application.Features.Finance.Queries.GetAccounts;
// Tambahkan using ini:
using FiTrack.Application.Features.Finance.Queries.GetAccountById; 
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FiTrack.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class AccountController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAccounts(CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetAccountsQuery(), cancellationToken);
        return Ok(result);
    }

    // --- TAMBAHKAN ENDPOINT INI ---
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetAccountByIdQuery(id);
        var result = await mediator.Send(query, cancellationToken);
        return Ok(result);
    }
    // ------------------------------

    [HttpPost]
    public async Task<IActionResult> CreateAccount([FromBody] CreateAccountCommand command, CancellationToken cancellationToken)
    {
        var id = await mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetAccounts), new { id }, new { Id = id }); // Anda bisa mengganti nameof(GetAccounts) dengan nameof(GetById) nanti
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAccount(Guid id, [FromBody] UpdateAccountCommand command, CancellationToken cancellationToken)
    {
        if (id != command.Id)
        {
            return BadRequest("ID mismatch");
        }

        await mediator.Send(command, cancellationToken);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAccount(Guid id, CancellationToken cancellationToken)
    {
        await mediator.Send(new DeleteAccountCommand(id), cancellationToken);
        return NoContent();
    }
}