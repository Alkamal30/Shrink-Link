
using Microsoft.EntityFrameworkCore;
using MediatR;
using ShrinkLink.LinkService.Domain.Data;
using ShrinkLink.LinkService.Domain.Entities;

namespace ShrinkLink.LinkService.Application.Features.AddLink;

public class AddLinkHandler : IRequestHandler<AddLinkCommand, Link>
{
	public AddLinkHandler(ILinkServiceContext context)
	{
		_context = context;
	}

	private readonly ILinkServiceContext _context;

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
