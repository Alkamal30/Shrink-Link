using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ShrinkLink.UserService.Domain.Data;
using ShrinkLink.UserService.Domain.Entities;
using ShrinkLink.UserService.Domain.Enums;

namespace ShrinkLink.UserService.Application.Features.Users;

public class CreateUserHandler(IUserServiceContext context, IPasswordHasher<User> passwordHasher) : IRequestHandler<CreateUserCommand, CreateUserResponse>
{
    private readonly IUserServiceContext _context = context;
    private readonly IPasswordHasher<User> _passwordHasher = passwordHasher;

    public async Task<CreateUserResponse> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
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

        return new CreateUserResponse(newUser.Id, newUser.Email);
    }
}
