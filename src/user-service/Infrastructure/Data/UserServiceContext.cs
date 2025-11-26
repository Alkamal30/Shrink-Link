using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using ShrinkLink.UserService.Domain.Data;
using ShrinkLink.UserService.Domain.Entities;
using ShrinkLink.UserService.Domain.Enums;

namespace ShrinkLink.UserService.Infrastructure.Data;


public class UserServiceContext(DbContextOptions<UserServiceContext> options) : DbContext(options), IUserServiceContext
{
    public static class SeedData
    {
        public static readonly Role UserRole = new() { Id = 1, Name = nameof(UserRoleEnum.User) };
        public static readonly Role AdminRole = new() { Id = 2, Name = nameof(UserRoleEnum.Admin) };

        public static readonly User DefaultUser;
        public static readonly User AdminUser;

        static SeedData()
        {
            DefaultUser = new User
            {
                Id = new Guid("53f72fc2-cbda-43fe-90b9-45ed571e4185"),
                Email = "user@user.com",
                PasswordHash = "AQAAAAIAAYagAAAAEEl9XcH2utjEVsSK57jhoxrThtc2z0kQ1hf/0a/E7qL+HO/7K6Bkav3KfSJOeA3WHw==" // admin123
            };
            AdminUser = new User
            {
                Id = new Guid("320c16ce-5e1c-40d6-83bb-53c7342ca773"),
                Email = "admin@admin.com",
                PasswordHash = "AQAAAAIAAYagAAAAEDliq5Roxa0gkptym2OdPjNXO4oKQX8XFRjXeN+2wUUjzOE4Uo3swfvqZwljE/In/w==" // user1234
            };
        }
    }

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
            .UsingEntity<UserRoleMap>(
                r => r.HasOne(ur => ur.Role).WithMany().HasForeignKey(ur => ur.RoleId),
                l => l.HasOne(ur => ur.User).WithMany().HasForeignKey(ur => ur.UserId),
                j => j.HasKey(ur => new { ur.UserId, ur.RoleId })
            );

        modelBuilder.Entity<Role>()
            .HasData(
                SeedData.UserRole,
                SeedData.AdminRole
            );

        modelBuilder.Entity<User>()
            .HasData(
                SeedData.DefaultUser,
                SeedData.AdminUser
            );

        modelBuilder.Entity<UserRoleMap>()
            .HasData(
                new UserRoleMap { UserId = SeedData.DefaultUser.Id, RoleId = (int)UserRoleEnum.User },
                new UserRoleMap { UserId = SeedData.AdminUser.Id, RoleId = (int)UserRoleEnum.User },
                new UserRoleMap { UserId = SeedData.AdminUser.Id, RoleId = (int)UserRoleEnum.Admin }
            );
    }
}