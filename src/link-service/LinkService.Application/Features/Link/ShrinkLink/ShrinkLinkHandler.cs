
using Microsoft.EntityFrameworkCore;
using MediatR;
using ShrinkLink.LinkService.Domain.Entities;
using ShrinkLink.LinkService.Domain.Services;
using ShrinkLink.LinkService.Infrastructure.Data;

namespace ShrinkLink.LinkService.Application.Features.ShrinkLink;

public class ShrinkLinkHandler : IRequestHandler<ShrinkLinkCommand, string>
{
	public ShrinkLinkHandler(LinkServiceContext context, IShortCodeService shortCodeService)
	{
		_context = context;
        _shortCodeService = shortCodeService;
	}

	private LinkServiceContext _context;
    private IShortCodeService _shortCodeService;

	public async Task<string> Handle(ShrinkLinkCommand request, CancellationToken cancellationToken)
	{
        Link newLink = new()
        {
            ShortUrl = string.Empty,
            OriginalUrl = request.OriginalUrl,
        };

        var entityEntry = await _context.Links.AddAsync(newLink);
        await _context.SaveChangesAsync();

        string shortCode = _shortCodeService.GenerateFromId(entityEntry.Entity.Id);
        entityEntry.Entity.ShortUrl = shortCode; 
        
        _context.Links.Update(entityEntry.Entity);
        await _context.SaveChangesAsync();

        return shortCode;
	}
}
