using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ShrinkLink.UserService.Domain.Data;
using ShrinkLink.UserService.Domain.Entities;

namespace ShrinkLink.UserService.Application.Features.Users;

public class RegisterUserHandler(IUserServiceContext context, IPasswordHasher<User> passwordHasher) : IRequestHandler<RegisterUserCommand>
{
    private readonly IUserServiceContext _context = context;
    private readonly IPasswordHasher<User> _passwordHasher = passwordHasher;

    public async Task Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var foundUser = await _context.Users.FirstOrDefaultAsync(x =>
            x.Email.Equals(request.Email, StringComparison.InvariantCultureIgnoreCase), cancellationToken);

        if(foundUser is not null) {
            throw new Exception("User with this Email address already exists");
        }

        var newUser = new User
        {
            Email = request.Email,
            PasswordHash = string.Empty,
        };

        newUser.PasswordHash = _passwordHasher.HashPassword(newUser, request.Password);

        await _context.Users.AddAsync(newUser, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
