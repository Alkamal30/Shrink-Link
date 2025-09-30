using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using MediatR;
using ShrinkLink.LinkService.Domain.Data;
using ShrinkLink.LinkService.Domain.Entities;

namespace ShrinkLink.LinkService.Application.Features.GetAllLinks;

public class GetAllLinksHandler : IRequestHandler<GetAllLinksQuery, IEnumerable<Link>>
{
	public GetAllLinksHandler(ILinkServiceContext context)
	{
		_context = context;
	}

	private ILinkServiceContext _context;

	public async Task<IEnumerable<Link>> Handle(GetAllLinksQuery request, CancellationToken cancellationToken)
	{
		return await _context.Links.ToListAsync();
	}
}
