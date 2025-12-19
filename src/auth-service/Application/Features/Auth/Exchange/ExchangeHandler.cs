using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using OpenIddict.Abstractions;
using ShrinkLink.AuthService.Infrastructure.Data.Entities;
using System.Security.Claims;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace ShrinkLink.AuthService.Application.Features.Auth.Exchange;

public class ExchangeHandler(UserManager<Identity> userManager) : IRequestHandler<ExchangeCommand, Result<ClaimsIdentity>>
{
    private readonly UserManager<Identity> _userManager = userManager;

    public async Task<Result<ClaimsIdentity>> Handle(ExchangeCommand request, CancellationToken cancellationToken)
    {
        var identity = new ClaimsIdentity(request.Principal.Claims,
            authenticationType: TokenValidationParameters.DefaultAuthenticationType,
            nameType: Claims.Name,
            roleType: Claims.Role
        );

        identity.SetClaim(Claims.Subject, await _userManager.GetUserIdAsync(request.User))
            .SetClaim(Claims.Email, await _userManager.GetEmailAsync(request.User))
            .SetClaim(Claims.Name, await _userManager.GetUserNameAsync(request.User))
            .SetClaim(Claims.PreferredUsername, await _userManager.GetUserNameAsync(request.User))
            .SetClaims(Claims.Role, [.. (await _userManager.GetRolesAsync(request.User))]);

        identity.SetDestinations(GetDestinations);

        return Result.Ok(identity);
    }

    private static IEnumerable<string> GetDestinations(Claim claim)
    {
        switch (claim.Type)
        {
            case Claims.Name or Claims.PreferredUsername:
                yield return Destinations.AccessToken;

                if (claim.Subject!.HasScope(Scopes.Profile))
                    yield return Destinations.IdentityToken;

                yield break;

            case Claims.Email:
                yield return Destinations.AccessToken;

                if (claim.Subject!.HasScope(Scopes.Email))
                    yield return Destinations.IdentityToken;

                yield break;

            case Claims.Role:
                yield return Destinations.AccessToken;

                if (claim.Subject!.HasScope(Scopes.Roles))
                    yield return Destinations.IdentityToken;

                yield break;

            case "AspNet.Identity.SecurityStamp": yield break;

            default:
                yield return Destinations.AccessToken;
                yield break;
        }
    }
}
