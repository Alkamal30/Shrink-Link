using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Identity;
using ShrinkLink.AuthService.Infrastructure.Data.Entities;

namespace ShrinkLink.AuthService.Application.Features.Auth.SignIn;

public class SignInHandler(UserManager<Identity> userManager, SignInManager<Identity> signInManager)
    : IRequestHandler<SignInCommand, Result>
{
    private readonly UserManager<Identity> _userManager = userManager;
    private readonly SignInManager<Identity> _signInManager = signInManager;

    public async Task<Result> Handle(SignInCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user is null)
            return Result.Fail("User is not exist");

        var ok = await _signInManager.CheckPasswordSignInAsync(user, request.Password, lockoutOnFailure: true);
        if (!ok.Succeeded)
            return Result.Fail("Password is incorrect");

        await _signInManager.SignInAsync(user, isPersistent: false);

        return Result.Ok();
    }
}
