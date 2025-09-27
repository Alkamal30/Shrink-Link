
using Microsoft.EntityFrameworkCore;
using MediatR;
using ShrinkLink.LinkService.Domain.Entities;
using ShrinkLink.LinkService.Infrastructure.Data;

namespace ShrinkLink.LinkService.Application.Features.DeleteLink;

public class DeleteLinkHandler : IRequestHandler<DeleteLinkCommand>
{
	public DeleteLinkHandler(LinkServiceContext context)
	{
		_context = context;
	}

	private LinkServiceContext _context;

	public async Task Handle(DeleteLinkCommand request, CancellationToken cancellationToken)
	{
        var entity = await _context.Links.FirstOrDefaultAsync(x => x.Id == request.Id);

        if (entity is not null)
        {
            _context.Links.Remove(entity);
            await _context.SaveChangesAsync();
        }
	}
}
