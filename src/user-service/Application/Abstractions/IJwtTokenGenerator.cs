using ShrinkLink.UserService.Domain.Entities;

namespace ShrinkLink.UserService.Application.Abstractions;

public interface IJwtTokenGenerator
{
    string GenerateJwtToken(User user, IEnumerable<Role> roles);
}
