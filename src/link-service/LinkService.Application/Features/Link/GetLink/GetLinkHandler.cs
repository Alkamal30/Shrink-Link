using Microsoft.EntityFrameworkCore;
using MediatR;
using ShrinkLink.LinkService.Domain.Entities;
using ShrinkLink.LinkService.Infrastructure.Data;

namespace ShrinkLink.LinkService.Application.Features.GetLink;

public class GetLinkHandler : IRequestHandler<GetLinkQuery, Link>
{
	public GetLinkHandler(LinkServiceContext context)
	{
		_context = context;
	}

	private LinkServiceContext _context;

	public async Task<Link> Handle(GetLinkQuery request, CancellationToken cancellationToken)
	{
		return await _context.Links.FirstOrDefaultAsync(x => x.Id == request.Id);
	}
}
