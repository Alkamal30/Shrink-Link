using MediatR;

namespace ShrinkLink.UserService.Application.Features.Users;

public record UpdateUserCommand(Guid Id, string? Email, string? Password) : IRequest;
