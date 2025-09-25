using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MediatR;
using ShrinkLink.LinkService;
using ShrinkLink.LinkService.Domain.Entities;
using ShrinkLink.LinkService.Queries;

namespace ShrinkLink.LinkService.Controllers;

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
	public async Task<IActionResult> Add(Link link)
	{
		link.Id = 0;

		await _context.Links.AddAsync(link);
		await _context.SaveChangesAsync();

		return Ok();
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
