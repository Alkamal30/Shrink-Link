using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Identity;
using ShrinkLink.AuthService.Infrastructure.Data.Entities;

namespace ShrinkLink.AuthService.Application.Features.Auth.SignUp;

public class SignUpHandler(UserManager<Identity> userManager) : IRequestHandler<SignUpCommand, Result>
{
    private readonly UserManager<Identity> _userManager = userManager;

    public async Task<Result> Handle(SignUpCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByNameAsync(request.Email);
        if (user is not null)
            return Result.Fail("The user alredy exists");

        user = new Identity { UserName = request.Email, Email = request.Email };
        var result = await _userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
            return Result.Fail(result.Errors.Select(x => x.Description));

        return Result.Ok();
    }
}
