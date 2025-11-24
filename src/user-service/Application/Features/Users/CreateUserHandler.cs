using MediatR;
using Microsoft.AspNetCore.Identity;
using ShrinkLink.UserService.Domain.Data;
using ShrinkLink.UserService.Domain.Entities;

namespace ShrinkLink.UserService.Application.Features.Users;

public class CreateUserHandler(IUserServiceContext context, IPasswordHasher<User> passwordHasher) : IRequestHandler<CreateUserCommand, CreateUserResponse>
{
    private readonly IUserServiceContext _userServiceContext = context;
    private readonly IPasswordHasher<User> _passwordHasher = passwordHasher;

    public async Task<CreateUserResponse> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var newUser = new User
        {
            Email = request.Email,
            PasswordHash = string.Empty,
        };

        newUser.PasswordHash = _passwordHasher.HashPassword(newUser, request.Password);

        await _userServiceContext.Users.AddAsync(newUser, cancellationToken);
        await _userServiceContext.SaveChangesAsync(cancellationToken);

        return new CreateUserResponse(newUser.Id, newUser.Email);
    }
}
