using MediatR;

namespace ShrinkLink.UserService.Application.Features.Users;

public record RegisterUserCommand(string Email, string Password) : IRequest;