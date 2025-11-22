using MediatR;
using Microsoft.EntityFrameworkCore;
using ShrinkLink.UserService.Domain.Data;
using ShrinkLink.UserService.Domain.Entities;

namespace ShrinkLink.UserService.Application.Features.Users;

public class GetUsersHandler(IUserServiceContext context) : IRequestHandler<GetUsersQuery, IEnumerable<User>>
{
    private readonly IUserServiceContext _context = context;

    public async Task<IEnumerable<User>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        return await _context.Users.ToListAsync(cancellationToken);
    }
}
