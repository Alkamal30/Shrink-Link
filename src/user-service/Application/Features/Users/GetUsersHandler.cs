using MediatR;
using Microsoft.EntityFrameworkCore;
using ShrinkLink.UserService.Domain.Data;
using ShrinkLink.UserService.Domain.Entities;

namespace ShrinkLink.UserService.Application.Features.Users;

public class GetUsersHandler(IUserServiceContext context) : IRequestHandler<GetUsersQuery, IEnumerable<GetUserResponse>>
{
    private readonly IUserServiceContext _context = context;

    public async Task<IEnumerable<GetUserResponse>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        return (await _context.Users.ToListAsync(cancellationToken)).Select(MapUserToResponse);
    }

    private GetUserResponse MapUserToResponse(User user)
    {
        return new GetUserResponse(user.Id, user.Email);
    }
}