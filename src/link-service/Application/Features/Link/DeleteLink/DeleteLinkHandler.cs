using Microsoft.EntityFrameworkCore;
using MediatR;
using ShrinkLink.LinkService.Domain.Data;
using ShrinkLink.LinkService.Domain.Entities;

namespace ShrinkLink.LinkService.Application.Features.DeleteLink;

public class DeleteLinkHandler : IRequestHandler<DeleteLinkCommand>
{
	public DeleteLinkHandler(ILinkServiceContext context)
	{
		_context = context;
	}

	private readonly ILinkServiceContext _context;

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
