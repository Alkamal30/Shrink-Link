using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShrinkLink.LinkService;
using ShrinkLink.LinkService.Models;

namespace ShrinkLink.LinkService.Controllers;

[ApiController]
public class LinkController : ControllerBase
{
	public LinkController(LinkServiceContext context)
	{
		_context = context;
	}

	private LinkServiceContext _context;

	[HttpGet]
	[Route("[controller]/")]
	public async Task<IEnumerable<Link>> GetAll()
	{		
		return await _context.Links.ToListAsync();
	}

	[HttpGet]
	[Route("[controller]/{id}")]
	public async Task<Link> Get(long id)
	{
		return await _context.Links.FirstOrDefaultAsync(x => x.Id == id);
	}

	[HttpPost]
	[Route("[controller]/")]
	public async Task<IActionResult> Add(Link link)
	{
		link.Id = 0;

		await _context.Links.AddAsync(link);
		await _context.SaveChangesAsync();

		return Ok();
	}

	[HttpPut]
	[Route("[controller]/")]
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
