using MediatR;

namespace ShrinkLink.UserService.Application.Features.Users;

public record MeUserQuery(Guid UserId) : IRequest<MeUserResponse>;
