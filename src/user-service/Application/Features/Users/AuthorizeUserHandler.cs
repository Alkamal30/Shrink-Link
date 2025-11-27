using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ShrinkLink.UserService.Application.Abstractions;
using ShrinkLink.UserService.Domain.Data;
using ShrinkLink.UserService.Domain.Entities;

namespace ShrinkLink.UserService.Application.Features.Users;

public class AuthorizeUserHandler(
    IUserServiceContext context,
    IPasswordHasher<User> passwordHasher,
    IJwtTokenGenerator jwtTokenGenerator
    ) : IRequestHandler<AuthorizeUserCommand, Result<string>>
{
    private static string IncorrectDataMessage => "Email or Password are incorrect.";

    private readonly IUserServiceContext _context = context;
    private readonly IPasswordHasher<User> _passwordHasher = passwordHasher;
    private readonly IJwtTokenGenerator _jwtTokenGenerator = jwtTokenGenerator;

    public async Task<Result<string>> Handle(AuthorizeUserCommand request, CancellationToken cancellationToken)
    {
        var normalizedEmail = request.Email.Trim().ToLowerInvariant();
        var user = await _context.Users
            .Include(x => x.Roles)
            .FirstOrDefaultAsync(x => x.Email == normalizedEmail, cancellationToken);

        if (user is null)
        {
            return Result.Fail(IncorrectDataMessage);
        }

        var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);

        if (result is PasswordVerificationResult.Failed)
        {
            return Result.Fail(IncorrectDataMessage);
        }

        if (result is PasswordVerificationResult.SuccessRehashNeeded)
        {
            user.PasswordHash = _passwordHasher.HashPassword(user, request.Password);

            _context.Users.Update(user);
            await _context.SaveChangesAsync(cancellationToken);
        }

        var jwtToken = _jwtTokenGenerator.GenerateJwtToken(user, user.Roles);

        return Result.Ok(jwtToken);
    }
}
