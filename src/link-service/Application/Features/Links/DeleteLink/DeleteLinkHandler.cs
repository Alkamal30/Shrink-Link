using MediatR;
using ShrinkLink.LinkService.Domain.Data;

namespace ShrinkLink.LinkService.Application.Features.Links.DeleteLink;

public class DeleteLinkHandler(ILogger<DeleteLinkHandler> logger, ILinkServiceContext context) : IRequestHandler<DeleteLinkCommand>
{
    private readonly ILogger<DeleteLinkHandler> _logger = logger;
    private readonly ILinkServiceContext _context = context;

    public async Task Handle(DeleteLinkCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Deleting link with ID: {Id}", request.Id);

        var entity = await _context.Links.FindAsync([request.Id], cancellationToken);

        if (entity is null)
        {
            _logger.LogWarning("Link with ID: {Id} not found for deletion", request.Id);
            return;
        }

        _context.Links.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("Successfully deleted link with ID: {Id}", request.Id);
    }
}
