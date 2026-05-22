using FiTrack.Application.Features.Finance.Commands.CreateAccount;
using FiTrack.Application.Features.Finance.Queries.GetAccounts;
using FiTrack.Application.Features.Finance.Commands.UpdateAccount; 
using FiTrack.Application.Features.Finance.Commands.DeleteAccount;
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
    public async Task<IActionResult> GetAccounts(CancellationToken ct)
    {
        var result = await mediator.Send(new GetAccountsQuery(), ct);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateAccount([FromBody] CreateAccountCommand command, CancellationToken ct)
    {
        var accountId = await mediator.Send(command, ct);
        return CreatedAtAction(nameof(GetAccounts), new { id = accountId }, new { Id = accountId });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAccount(Guid id, [FromBody] UpdateAccountCommand command, CancellationToken ct)
    {
        // Validasi keamanan agar ID di URL sama dengan ID di payload body
        if (id != command.Id)
        {
            return BadRequest("Account ID mismatch between URL and body.");
        }

        await mediator.Send(command, ct);
        return NoContent(); // Mengembalikan 204 No Content (standar sukses untuk Update)
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAccount(Guid id, CancellationToken ct)
    {
        await mediator.Send(new DeleteAccountCommand(id), ct);
        return NoContent(); // Mengembalikan 204 No Content (standar sukses untuk Delete)
    }
}