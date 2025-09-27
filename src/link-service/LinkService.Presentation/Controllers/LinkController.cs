using System;
using System.Collections.Generic;
using MediatR;
using Microsoft.AspNetCore.Mvc; 
using Microsoft.EntityFrameworkCore;
using ShrinkLink.LinkService.Application.Features.AddLink;
using ShrinkLink.LinkService.Application.Features.GetAllLinks;
using ShrinkLink.LinkService.Application.Features.GetLink;
using ShrinkLink.LinkService.Domain.Entities;
using ShrinkLink.LinkService.Infrastructure.Data;


namespace ShrinkLink.LinkService.Presentation.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class LinkController : ControllerBase
{
	public LinkController(LinkServiceContext context, IMediator mediator)
	{
		_context = context;
		_mediator = mediator;
	}

	private LinkServiceContext _context;
	private IMediator _mediator;

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
	public async Task<IActionResult> Update(Link link)
	{
		Link entity = await _context.Links.FirstOrDefaultAsync(x => x.Id == link.Id);

		if (entity is not null)
		{
			entity.ShortUrl = link.ShortUrl;
			entity.OriginalUrl = link.OriginalUrl;
			await _context.SaveChangesAsync();

			return Ok();
		}

		return BadRequest();
	}

	[HttpDelete]
	public async Task<IActionResult> Delete(long id)
	{
		Link entity = await _context.Links.FirstOrDefaultAsync(x => x.Id == id);

		if (entity is not null)
		{
			_context.Links.Remove(entity);
			await _context.SaveChangesAsync();

			return Ok();
		}

		return BadRequest();
	}
}
