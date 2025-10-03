using Microsoft.EntityFrameworkCore;
using MediatR;
using ShrinkLink.LinkService.Domain.Data;
using ShrinkLink.LinkService.Domain.Entities;

namespace ShrinkLink.LinkService.Application.Features.GetLink;

public class GetLinkHandler : IRequestHandler<GetLinkQuery, Link>
{
	public GetLinkHandler(ILinkServiceContext context)
	{
		_context = context;
	}

	private readonly ILinkServiceContext _context;

	public async Task<Link> Handle(GetLinkQuery request, CancellationToken cancellationToken)
	{
		return await _context.Links.FirstOrDefaultAsync(x => x.Id == request.Id);
	}
}
