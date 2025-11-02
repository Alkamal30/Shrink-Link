using Microsoft.EntityFrameworkCore;
using ShrinkLink.LinkService.Domain.Entities;

namespace ShrinkLink.LinkService.Domain.Data;

public interface ILinkServiceContext
{
    DbSet<Link> Links { get; }
    Task<int> SaveChangesAsync();
}
