using MediatR;

namespace ShrinkLink.UserService.Application.Features.Users;

public record DeleteUserCommand(Guid Id) : IRequest;
