using MediatR;
using ShrinkLink.UserService.Domain.Entities;

namespace ShrinkLink.UserService.Application.Features.Users;

public record CreateUserCommand(string Email, string Password) : IRequest<CreateUserResponse>;