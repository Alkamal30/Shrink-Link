using Microsoft.EntityFrameworkCore;
using ShrinkLink.LinkService.Models;

namespace ShrinkLink.LinkService;

public class LinkServiceContext : DbContext
{
	public LinkServiceContext(DbContextOptions<LinkServiceContext> options) : base(options)
        {
        }

	public DbSet<Link> Links { get; set; }
}
