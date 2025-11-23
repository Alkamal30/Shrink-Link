using MediatR;
using ShrinkLink.UserService.Domain.Data;

namespace ShrinkLink.UserService.Application.Features.Users;

public class UpdateUserHandler(IUserServiceContext context) : IRequestHandler<UpdateUserCommand>
{
    private readonly IUserServiceContext _context = context;

    public async Task Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FindAsync([request.Id], cancellationToken)
            ?? throw new Exception($"User {request.Id} is not found!");

        user.Email = request.Email ?? user.Email;
        user.Password = request.Password ?? user.Password;

        _context.Users.Update(user);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
