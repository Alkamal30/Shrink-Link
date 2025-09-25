using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using MediatR;
using ShrinkLink.LinkService;
using ShrinkLink.LinkService.Domain.Entities;

namespace ShrinkLink.LinkService.Queries;

public class GetAllLinksHandler : IRequestHandler<GetAllLinksQuery, IEnumerable<Link>>
{
	public GetAllLinksHandler(LinkServiceContext context)
	{
		_context = context;
	}

	private LinkServiceContext _context;

	public async Task<IEnumerable<Link>> Handle(GetAllLinksQuery request, CancellationToken cancellationToken)
	{
		return await _context.Links.ToListAsync();
	}
}
