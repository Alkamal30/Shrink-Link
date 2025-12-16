using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ShrinkLink.AuthService.Domain.Data;
using ShrinkLink.AuthService.Infrastructure.Data.Entities;

namespace ShrinkLink.AuthService.Infrastructure.Data;

public class AuthServiceDbContext(DbContextOptions<AuthServiceDbContext> options)
    : IdentityDbContext<Identity, IdentityRole<Guid>, Guid>(options), IAuthServiceDbContext
{
    public new async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await base.SaveChangesAsync(cancellationToken);
    }
}
