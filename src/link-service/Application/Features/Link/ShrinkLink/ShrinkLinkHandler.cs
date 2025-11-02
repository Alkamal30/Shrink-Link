using Microsoft.EntityFrameworkCore;
using MediatR;
using ShrinkLink.LinkService.Domain.Data;
using ShrinkLink.LinkService.Domain.Entities;
using ShrinkLink.LinkService.Domain.Services;

namespace ShrinkLink.LinkService.Application.Features.ShrinkLink;

public class ShrinkLinkHandler : IRequestHandler<ShrinkLinkCommand, string>
{
	public ShrinkLinkHandler(ILinkServiceContext context, IShortCodeService shortCodeService)
	{
		_context = context;
        _shortCodeService = shortCodeService;
	}

	private readonly ILinkServiceContext _context;
    private readonly IShortCodeService _shortCodeService;

	public async Task<string> Handle(ShrinkLinkCommand request, CancellationToken cancellationToken)
	{
        var newLink = new Link()
        {
            ShortUrl = string.Empty,
            OriginalUrl = request.OriginalUrl,
        };

        var entityEntry = await _context.Links.AddAsync(newLink);
        await _context.SaveChangesAsync();

        var shortCode = _shortCodeService.GenerateFromId(entityEntry.Entity.Id);
        entityEntry.Entity.ShortUrl = shortCode; 
        
        await _context.SaveChangesAsync();

        return shortCode;
	}
}
