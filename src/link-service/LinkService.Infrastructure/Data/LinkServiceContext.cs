using Microsoft.EntityFrameworkCore;
using ShrinkLink.LinkService.Domain.Data;
using ShrinkLink.LinkService.Domain.Entities;

namespace ShrinkLink.LinkService.Infrastructure.Data;

public class LinkServiceContext : DbContext, ILinkServiceContext
{
	public LinkServiceContext(DbContextOptions<LinkServiceContext> options) : base(options)
    {
    }

	public DbSet<Link> Links => Set<Link>();

    public async Task<int> SaveChangesAsync()
        => await base.SaveChangesAsync();
}
