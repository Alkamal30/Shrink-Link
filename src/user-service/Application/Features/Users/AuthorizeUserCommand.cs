using MediatR;

namespace ShrinkLink.UserService.Application.Features.Users;

public record AuthorizeUserCommand(string Email, string Password) : IRequest<bool>;
