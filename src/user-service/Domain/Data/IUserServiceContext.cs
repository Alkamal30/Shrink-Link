using Microsoft.EntityFrameworkCore;
using ShrinkLink.UserService.Domain.Entities;

namespace ShrinkLink.UserService.Domain.Data;

public interface IUserServiceContext
{
    DbSet<User> Users { get; }
    DbSet<Role> Roles { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}