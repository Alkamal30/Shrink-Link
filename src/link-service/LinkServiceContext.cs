using Microsoft.EntityFrameworkCore;
using ShrinkLink.LinkService.Domain.Entities;

namespace ShrinkLink.LinkService;

public class LinkServiceContext : DbContext
{
	public LinkServiceContext(DbContextOptions<LinkServiceContext> options) : base(options)
        {
        }

	public DbSet<Link> Links { get; set; }
}
