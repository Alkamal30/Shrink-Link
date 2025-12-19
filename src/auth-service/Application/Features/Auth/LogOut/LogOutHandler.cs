using MediatR;
using Microsoft.AspNetCore.Identity;
using ShrinkLink.AuthService.Infrastructure.Data.Entities;

namespace ShrinkLink.AuthService.Application.Features.Auth.LogOut;

public class LogOutHandler(SignInManager<Identity> signInManager) : IRequestHandler<LogOutCommand>
{
    private readonly SignInManager<Identity> _signInManager = signInManager;

    public async Task Handle(LogOutCommand request, CancellationToken cancellationToken)
    {
        await _signInManager.SignOutAsync();
    }
}
