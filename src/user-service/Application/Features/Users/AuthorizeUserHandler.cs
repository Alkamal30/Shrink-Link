using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ShrinkLink.UserService.Domain.Data;
using ShrinkLink.UserService.Domain.Entities;

namespace ShrinkLink.UserService.Application.Features.Users;

public class AuthorizeUserHandler(IUserServiceContext context, IPasswordHasher<User> passwordHasher) : IRequestHandler<AuthorizeUserCommand, bool>
{
    private readonly IUserServiceContext _context = context;
    private readonly IPasswordHasher<User> _passwordHasher = passwordHasher;

    public async Task<bool> Handle(AuthorizeUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x =>
            x.Email.Equals(request.Email, StringComparison.InvariantCultureIgnoreCase), cancellationToken)
            ?? throw new Exception("There is no user with this Email address");

        var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);

        return result switch
        {
            PasswordVerificationResult.Success or PasswordVerificationResult.SuccessRehashNeeded => true,
            _ => false,
        };
    }
}
