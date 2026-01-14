using MediatR;
using ShrinkLink.LinkService.Domain.Data;
using ShrinkLink.LinkService.Domain.Entities;
using ShrinkLink.LinkService.Domain.Services;

namespace ShrinkLink.LinkService.Application.Features.Links.ShrinkLink;

using Microsoft.Extensions.Logging;

public class ShrinkLinkHandler(
    ILogger<ShrinkLinkHandler> logger,
    ILinkServiceContext context,
    IShortCodeService shortCodeService)
    : IRequestHandler<ShrinkLinkCommand, string>
{
    private readonly ILogger<ShrinkLinkHandler> _logger = logger;
    private readonly ILinkServiceContext _context = context;
    private readonly IShortCodeService _shortCodeService = shortCodeService;

    public async Task<string> Handle(ShrinkLinkCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Shrinking link: {OriginalUrl}", request.OriginalUrl);

        var newLink = new Link()
        {
            ShortUrl = string.Empty,
            OriginalUrl = request.OriginalUrl,
        };

        var entityEntry = await _context.Links.AddAsync(newLink, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        var shortCode = _shortCodeService.GenerateFromId(entityEntry.Entity.Id);
        entityEntry.Entity.ShortUrl = shortCode;

        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Link shrunk successfully. Short code: {ShortCode}", shortCode);

        return shortCode;
    }
}
