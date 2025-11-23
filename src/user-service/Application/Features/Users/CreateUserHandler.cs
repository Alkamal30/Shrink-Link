using MediatR;
using ShrinkLink.UserService.Domain.Data;
using ShrinkLink.UserService.Domain.Entities;

namespace ShrinkLink.UserService.Application.Features.Users;

public class CreateUserHandler(IUserServiceContext context) : IRequestHandler<CreateUserCommand, CreateUserResponse>
{
    private readonly IUserServiceContext _userServiceContext = context;

    public async Task<CreateUserResponse> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var newUser = new User
        {
            Email = request.Email,
            Password = request.Password,
        };

        await _userServiceContext.Users.AddAsync(newUser);
        await _userServiceContext.SaveChangesAsync(cancellationToken);

        return new CreateUserResponse(newUser.Id, newUser.Email);
    }
}
