using Microsoft.EntityFrameworkCore;
using ShrinkLink.UserService.Domain.Entities;

namespace ShrinkLink.UserService.Domain.Data;

public interface IUserServiceContext
{
    DbSet<User> Users { get; }
    Task<int> SaveChangesAsync();
}