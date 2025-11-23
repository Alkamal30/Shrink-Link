using MediatR;
using ShrinkLink.UserService.Domain.Data;

namespace ShrinkLink.UserService.Application.Features.Users;

public class DeleteUserHandler(IUserServiceContext context) : IRequestHandler<DeleteUserCommand>
{
    private readonly IUserServiceContext _context = context;

    public async Task Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FindAsync([request.Id], cancellationToken)
            ?? throw new Exception($"User {request.Id} is not found!");

        _context.Users.Remove(user);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
