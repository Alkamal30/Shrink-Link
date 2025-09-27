
using Microsoft.EntityFrameworkCore;
using MediatR;
using ShrinkLink.LinkService.Domain.Entities;
using ShrinkLink.LinkService.Infrastructure.Data;

namespace ShrinkLink.LinkService.Application.Features.AddLink;

public class AddLinkHandler : IRequestHandler<AddLinkCommand, Link>
{
	public AddLinkHandler(LinkServiceContext context)
	{
		_context = context;
	}

	private LinkServiceContext _context;

	public async Task<Link> Handle(AddLinkCommand request, CancellationToken cancellationToken)
	{
        Link newLink = new()
        {
            ShortUrl = request.ShortCode,
            OriginalUrl = request.OriginalUrl,
        };

        var entityEntry = await _context.Links.AddAsync(newLink);
        await _context.SaveChangesAsync();

        return entityEntry.Entity;
	}
}
