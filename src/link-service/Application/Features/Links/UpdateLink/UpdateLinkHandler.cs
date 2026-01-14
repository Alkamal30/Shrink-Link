using Microsoft.EntityFrameworkCore;
using MediatR;
using ShrinkLink.LinkService.Domain.Data;
using ShrinkLink.LinkService.Domain.Entities;

namespace ShrinkLink.LinkService.Application.Features.Links.UpdateLink;

using Microsoft.Extensions.Logging;

public class UpdateLinkHandler(ILogger<UpdateLinkHandler> logger, ILinkServiceContext context) : IRequestHandler<UpdateLinkCommand>
{
    private readonly ILogger<UpdateLinkHandler> _logger = logger;
    private readonly ILinkServiceContext _context = context;

    public async Task Handle(UpdateLinkCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Links.FindAsync([request.Id], cancellationToken);

        if (entity is null)
        {
            _logger.LogWarning("Link with ID {LinkId} not found.", request.Id);
            return;
        }

        entity.ShortUrl = request.ShortCode;
        entity.OriginalUrl = request.OriginalUrl;

        _context.Links.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Link with ID {LinkId} updated successfully.", request.Id);
    }
}
