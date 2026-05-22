using MediatR;
using FiTrack.Application.Features.Finance.Commands.CreateCategory;
using FiTrack.Application.Features.Finance.Commands.UpdateCategory;
using FiTrack.Application.Features.Finance.Commands.DeleteCategory;
using FiTrack.Application.Features.Finance.Queries.GetCategories;
using FiTrack.Application.Features.Finance.Queries.GetCategoryById;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FiTrack.API.Controllers;

[ApiController]
[Route("api/categories")]
[Authorize]
public class CategoryController(ISender mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetCategories()
    {
        var result = await mediator.Send(new GetCategoriesQuery());
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetCategoryById(Guid id)
    {
        var result = await mediator.Send(new GetCategoryByIdQuery(id));
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryCommand command)
    {
        var result = await mediator.Send(command);
        return Ok(result);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateCategory(Guid id, [FromBody] UpdateCategoryCommand command)
    {
        if (id != command.Id) return BadRequest();
        var result = await mediator.Send(command);
        return Ok(result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteCategory(Guid id)
    {
        await mediator.Send(new DeleteCategoryCommand(id));
        return NoContent();
    }
}