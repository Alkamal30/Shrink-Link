using MediatR;

namespace ShrinkLink.AuthService.Application.Features.Auth.LogOut;

public record LogOutCommand : IRequest;