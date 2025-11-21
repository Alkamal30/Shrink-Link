using MediatR;
using ShrinkLink.UserService.Domain.Entities;

namespace ShrinkLink.UserService.Application.Features.Users;

public record GetUserByIdQuery(Guid Id) : IRequest<User?>;