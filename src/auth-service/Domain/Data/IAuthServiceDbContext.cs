namespace ShrinkLink.AuthService.Domain.Data;

public interface IAuthServiceDbContext
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
