using MediatR;
using ShrinkLink.UserService.Domain.Data;
using ShrinkLink.UserService.Domain.Entities;

namespace ShrinkLink.UserService.Application.Features.Users;

public class GetUserByIdHandler(IUserServiceContext context) : IRequestHandler<GetUserByIdQuery, User?>
{
    private readonly IUserServiceContext _context = context;

    public async Task<User?> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        return await _context.Users.FindAsync([request.Id], cancellationToken: cancellationToken);
    }
}