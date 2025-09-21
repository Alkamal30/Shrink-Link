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
}
