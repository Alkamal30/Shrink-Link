using MediatR;
using Microsoft.AspNetCore.Mvc;
using ShrinkLink.UserService.Application.Features.Users;

namespace ShrinkLink.UserService.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController(ISender sender) : ControllerBase
{
    private readonly ISender _sender = sender;

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterUserCommand command, CancellationToken cancellationToken)
    {
        await _sender.Send(command, cancellationToken);

        return Ok();
    }

    [HttpPost("authorize")]
    public async Task<IActionResult> Authorize(AuthorizeUserCommand command, CancellationToken cancellationToken)
    {
        bool result = await _sender.Send(command, cancellationToken);

        if(!result)
        {
            return BadRequest();
        }

        return Ok();
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var query = new GetUsersQuery();

        var result = await _sender.Send(query, cancellationToken);

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetUserByIdQuery(id);

        var result = await _sender.Send(query, cancellationToken);

        if (result is null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateUserCommand command, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(command, cancellationToken);

        if (result is null)
        {
            return BadRequest();
        }

        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut]
    public async Task<IActionResult> Update(UpdateUserCommand command, CancellationToken cancellationToken)
    {
        await _sender.Send(command, cancellationToken);

        return NoContent();
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(DeleteUserCommand command, CancellationToken cancellationToken)
    {
        await _sender.Send(command, cancellationToken);

        return NoContent();
    }
}