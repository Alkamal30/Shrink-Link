using MediatR;
using Microsoft.AspNetCore.Mvc;
using ShrinkLink.UserService.Application.Features.Users;
using System.Threading.Tasks;

namespace ShrinkLink.UserService.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController(ISender sender) : ControllerBase
{
    private readonly ISender _sender = sender;

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
}