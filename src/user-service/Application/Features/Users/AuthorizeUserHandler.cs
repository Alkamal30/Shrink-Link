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
            ?? throw new Exception("Email or Password is incorrect");

        var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);

        if (result is PasswordVerificationResult.SuccessRehashNeeded)
        {
            user.PasswordHash = _passwordHasher.HashPassword(user, request.Password);

            _context.Users.Update(user);
            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }

        return result is PasswordVerificationResult.Success;
    }
}
