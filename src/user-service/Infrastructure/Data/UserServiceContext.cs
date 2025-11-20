using Microsoft.EntityFrameworkCore;
using ShrinkLink.UserService.Domain.Data;
using ShrinkLink.UserService.Domain.Entities;

namespace ShrinkLink.UserService.Infrastructure.Data;

public class UserServiceContext(DbContextOptions<UserServiceContext> options) : DbContext(options), IUserServiceContext
{
    public DbSet<User> Users => Set<User>();
    
    public async Task<int> SaveChangesAsync()
    {
        return await base.SaveChangesAsync();
    }
}