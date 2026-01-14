using MediatR;
using Microsoft.AspNetCore.Mvc;
using ShrinkLink.LinkService.Application.Features.Links.GetAllLinks;
using ShrinkLink.LinkService.Application.Features.Links.GetLink;
using ShrinkLink.LinkService.Application.Features.Links.UpdateLink;
using ShrinkLink.LinkService.Application.Features.Links.DeleteLink;
using ShrinkLink.LinkService.Application.Features.Links.ShrinkLink;
using ShrinkLink.LinkService.Domain.Entities;
using ShrinkLink.LinkService.Application.Features.Links.AddLink;

namespace ShrinkLink.LinkService.Presentation.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class LinkController : ControllerBase
{
    public LinkController(IMediator mediator)
    {
        _mediator = mediator;
    }

    private readonly IMediator _mediator;

    [HttpPost("shrink")]
    public async Task<ActionResult<string>> Shrink(string url)
    {
        var result = await _mediator.Send(new ShrinkLinkCommand(url));

        return Ok(result);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Link>>> GetAll()
    {
        var result = await _mediator.Send(new GetAllLinksQuery());

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Link>> Get(long id)
    {
        var result = await _mediator.Send(new GetLinkQuery(id));

        if (result is null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<Link>> Add(AddLinkCommand request)
    {
        var result = await _mediator.Send(request);

        if (result is null)
        {
            return BadRequest();
        }

        return Ok(result);
    }

    [HttpPut]
    public async Task<ActionResult> Update(UpdateLinkCommand request)
    {
        await _mediator.Send(request);

        return Ok();
    }

    [HttpDelete]
    public async Task<ActionResult> Delete(long id)
    {
        await _mediator.Send(new DeleteLinkCommand(id));

        return Ok();
    }
}
