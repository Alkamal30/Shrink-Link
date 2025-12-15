using MediatR;
using ShrinkLink.UserService.Domain.Data;

namespace ShrinkLink.UserService.Application.Features.Users;

public class MeUserHandler(IUserServiceContext context) : IRequestHandler<MeUserQuery, MeUserResponse>
{
    private readonly IUserServiceContext _context = context;

    public async Task<MeUserResponse> Handle(MeUserQuery request, CancellationToken cancellationToken)
    {
        var authenticatedUser = await _context.Users.FindAsync([request.UserId], cancellationToken);

        return new MeUserResponse(
            authenticatedUser is not null,
            authenticatedUser?.Email
        );
    }
}
