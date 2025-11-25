using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ShrinkLink.UserService.Domain.Data;
using ShrinkLink.UserService.Domain.Entities;
using ShrinkLink.UserService.Domain.Enums;

namespace ShrinkLink.UserService.Application.Features.Users;

public class RegisterUserHandler(IUserServiceContext context, IPasswordHasher<User> passwordHasher) : IRequestHandler<RegisterUserCommand, Result>
{
    private readonly IUserServiceContext _context = context;
    private readonly IPasswordHasher<User> _passwordHasher = passwordHasher;

    public async Task<Result> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var normalizedEmail = request.Email.Trim().ToLowerInvariant();
        var isExist = await _context.Users.AnyAsync(x => x.Email == normalizedEmail, cancellationToken);

        if (isExist)
        {
            return Result.Fail("User with this Email address alredy exists.");
        }

        var newUser = new User
        {
            Email = request.Email,
            PasswordHash = string.Empty,
        };

        newUser.PasswordHash = _passwordHasher.HashPassword(newUser, request.Password);

        var userRole = await _context.Roles.FirstAsync(x => x.Id == (int)UserRoleEnum.User, cancellationToken);
        newUser.Roles.Add(userRole);

        await _context.Users.AddAsync(newUser, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Ok();
    }
}
