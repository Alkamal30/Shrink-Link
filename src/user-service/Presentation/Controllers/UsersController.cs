using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShrinkLink.UserService.Application.Features.Users;
using ShrinkLink.UserService.Domain.Enums;

namespace ShrinkLink.UserService.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = nameof(UserRoleEnum.Admin))]
public class UsersController(ISender sender) : ControllerBase
{
    private readonly ISender _sender = sender;

    [HttpPost(nameof(Register))]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] RegisterUserCommand command, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        Result result = await _sender.Send(command, cancellationToken);

        if (result.IsFailed)
        {
            return BadRequest(result.Errors.Select(x => x.Message));
        }

        return NoContent();
    }

    [HttpPost(nameof(Authorize))]
    [AllowAnonymous]
    public async Task<IActionResult> Authorize([FromBody] AuthorizeUserCommand command, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        Result<string> result = await _sender.Send(command, cancellationToken);

        if(result.IsFailed)
        {
            return Unauthorized(result.Errors.Select(x => x.Message));
        }

        return Ok(new { Token = result.Value });
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var query = new GetUsersQuery();

        var result = await _sender.Send(query, cancellationToken);

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken cancellationToken)
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
    public async Task<IActionResult> Create([FromBody] CreateUserCommand command, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(command, cancellationToken);

        if (result is null)
        {
            return BadRequest();
        }

        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateUserCommand command, CancellationToken cancellationToken)
    {
        await _sender.Send(command, cancellationToken);

        return NoContent();
    }

    [HttpDelete]
    public async Task<IActionResult> Delete([FromBody] DeleteUserCommand command, CancellationToken cancellationToken)
    {
        await _sender.Send(command, cancellationToken);

        return NoContent();
    }
}