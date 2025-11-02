using Microsoft.EntityFrameworkCore;
using MediatR;
using ShrinkLink.LinkService.Domain.Data;
using ShrinkLink.LinkService.Domain.Entities;

namespace ShrinkLink.LinkService.Application.Features.UpdateLink;

public class UpdateLinkHandler : IRequestHandler<UpdateLinkCommand>
{
	public UpdateLinkHandler(ILinkServiceContext context)
	{
		_context = context;
	}

	private readonly ILinkServiceContext _context;

	public async Task Handle(UpdateLinkCommand request, CancellationToken cancellationToken)
	{
        var entity = await _context.Links.FirstOrDefaultAsync(x => x.Id == request.Id);

        if (entity is not null)
        {
            entity.ShortUrl = request.ShortCode;
            entity.OriginalUrl = request.OriginalUrl;

            _context.Links.Update(entity);
            await _context.SaveChangesAsync();
        }
	}
}
