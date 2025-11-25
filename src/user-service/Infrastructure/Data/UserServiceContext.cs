using Microsoft.EntityFrameworkCore;
using ShrinkLink.UserService.Domain.Data;
using ShrinkLink.UserService.Domain.Entities;
using ShrinkLink.UserService.Domain.Enums;

namespace ShrinkLink.UserService.Infrastructure.Data;

public class UserServiceContext(DbContextOptions<UserServiceContext> options) : DbContext(options), IUserServiceContext
{
    public DbSet<User> Users => Set<User>();

    public DbSet<Role> Roles => Set<Role>();

    public new async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .HasIndex(x => x.Email)
            .IsUnique();

        modelBuilder.Entity<User>()
            .HasMany(x => x.Roles)
            .WithMany(x => x.Users)
            .UsingEntity(x => x.ToTable("UserRoleMap"));

        modelBuilder.Entity<Role>()
            .HasData(
                new Role { Id = 1, Name = UserRoleEnum.User.ToString() },
                new Role { Id = 2, Name = UserRoleEnum.Admin.ToString() }
            );
    }
}