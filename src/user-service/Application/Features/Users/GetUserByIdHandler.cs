using MediatR;
using ShrinkLink.UserService.Domain.Data;
using ShrinkLink.UserService.Domain.Entities;

namespace ShrinkLink.UserService.Application.Features.Users;

public class GetUserByIdHandler(IUserServiceContext context) : IRequestHandler<GetUserByIdQuery, GetUserResponse?>
{
    private readonly IUserServiceContext _context = context;

    public async Task<GetUserResponse?> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var foundUser = await _context.Users.FindAsync([request.Id], cancellationToken: cancellationToken);

        if (foundUser is null)
            return null;

        return new GetUserResponse(foundUser.Id, foundUser.Email);
    }
}