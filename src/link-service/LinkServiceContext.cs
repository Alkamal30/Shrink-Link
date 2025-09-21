using Microsoft.EntityFrameworkCore;
using ShrinkLink.LinkService.Models;

namespace ShrinkLink.LinkService;

public class LinkServiceContext : DbContext
{
	public DbSet<Link> Links { get; set; }
}
