using Microsoft.EntityFrameworkCore;
using MediatR;
using ShrinkLink.LinkService.Domain.Data;
using ShrinkLink.LinkService.Domain.Entities;

namespace ShrinkLink.LinkService.Application.Features.Links.GetAllLinks;

using Microsoft.Extensions.Logging;

public class GetAllLinksHandler(ILogger<GetAllLinksHandler> logger, ILinkServiceContext context)
    : IRequestHandler<GetAllLinksQuery, IEnumerable<Link>>
{
    private readonly ILogger<GetAllLinksHandler> _logger = logger;
    private readonly ILinkServiceContext _context = context;

    public async Task<IEnumerable<Link>> Handle(GetAllLinksQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Fetching all links");
        return await _context.Links.ToListAsync(cancellationToken);
    }
}
