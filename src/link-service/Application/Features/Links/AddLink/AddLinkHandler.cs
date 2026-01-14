using MediatR;
using ShrinkLink.LinkService.Domain.Data;
using ShrinkLink.LinkService.Domain.Entities;

namespace ShrinkLink.LinkService.Application.Features.Links.AddLink;

public class AddLinkHandler(ILogger<AddLinkHandler> logger, ILinkServiceContext context) : IRequestHandler<AddLinkCommand, Link>
{
    private readonly ILogger<AddLinkHandler> _logger = logger;
    private readonly ILinkServiceContext _context = context;

    public async Task<Link> Handle(AddLinkCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Adding new link with short code: {ShortCode}", request.ShortCode);

        var newLink = new Link()
        {
            ShortUrl = request.ShortCode,
            OriginalUrl = request.OriginalUrl,
        };

        var entityEntry = await _context.Links.AddAsync(newLink, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Successfully added link with ID: {Id}", entityEntry.Entity.Id);

        return entityEntry.Entity;
    }
}
