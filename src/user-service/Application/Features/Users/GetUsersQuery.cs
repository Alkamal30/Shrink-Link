using MediatR;
using ShrinkLink.UserService.Domain.Entities;

namespace ShrinkLink.UserService.Application.Features.Users;

public record GetUsersQuery : IRequest<IEnumerable<GetUserResponse>>;