using MediatR;
using ShrinkLink.LinkService.Domain.Data;
using ShrinkLink.LinkService.Domain.Entities;

namespace ShrinkLink.LinkService.Application.Features.Links.GetLink;

using Microsoft.Extensions.Logging;

public class GetLinkHandler(ILogger<GetLinkHandler> logger, ILinkServiceContext context) : IRequestHandler<GetLinkQuery, Link?>
{
    private readonly ILogger<GetLinkHandler> _logger = logger;
    private readonly ILinkServiceContext _context = context;

    public async Task<Link?> Handle(GetLinkQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Fetching link with ID: {LinkId}", request.Id);
        return await _context.Links.FindAsync([request.Id], cancellationToken);
    }
}
