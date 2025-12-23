using MediatR;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using ShrinkLink.AuthService.Application.Features.Auth.Authorize;
using ShrinkLink.AuthService.Application.Features.Auth.Exchange;
using ShrinkLink.AuthService.Application.Features.Auth.LogOut;
using ShrinkLink.AuthService.Application.Features.Auth.SignIn;
using ShrinkLink.AuthService.Application.Features.Auth.SignUp;
using ShrinkLink.AuthService.Infrastructure.Data.Entities;
using System.Security.Claims;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace ShrinkLink.AuthService.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(
    UserManager<Identity> userManager,
    SignInManager<Identity> signInManager,
    IOpenIddictApplicationManager applicationManager,
    IOpenIddictAuthorizationManager authorizationManager,
    IOpenIddictScopeManager scopeManager,
    ISender sender
) : ControllerBase
{
    private readonly UserManager<Identity> _userManager = userManager;
    private readonly SignInManager<Identity> _signInManager = signInManager;
    private readonly IOpenIddictApplicationManager _applicationManager = applicationManager;
    private readonly IOpenIddictAuthorizationManager _authorizationManager = authorizationManager;
    private readonly IOpenIddictScopeManager _scopeManager = scopeManager;
    private readonly ISender _sender = sender;

    [HttpGet("[action]")]
    public async Task<IActionResult> SignIn(string? returnUrl)
    {
        return Content($"""
                <div style="width: 100%; height: 100%; display: flex; justify-content: center; align-items: center;">
                    <form method="post" action="/api/auth/{nameof(SignIn)}" style="display: flex; flex-direction: column; gap: 16px;">
                        <input name="email" placeholder="email" />
                        <input name="password" type="password" placeholder="password" />
                        <input type="hidden" name="returnUrl" value="{System.Net.WebUtility.HtmlEncode(returnUrl ?? "/")}" />
                        <button type="submit">Login</button>
                    </form>
                </div>
            """, "text/html"
        );
    }

    [HttpGet("[action]")]
    public async Task<IActionResult> LogOut(CancellationToken cancellationToken)
    {
        await _sender.Send(new LogOutCommand(), cancellationToken);

        return SignOut(
            authenticationSchemes: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme
        );
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> SignIn([FromForm] SignInCommand request, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return ValidationProblem(ModelState);

        var result = await _sender.Send(request, cancellationToken);

        if (result.IsFailed)
            return Unauthorized();

        return Redirect(request.ReturnUrl ?? "/");
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> SignUp([FromBody] SignUpCommand request, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return ValidationProblem(ModelState);

        var result = await _sender.Send(request, cancellationToken);

        if (result.IsFailed)
            return BadRequest(result.Errors.Select(x => x.Message));

        return NoContent();
    }

    [HttpGet("connect/authorize")]
    public async Task<IActionResult> Authorize(CancellationToken cancellationToken)
    {
        var openIddictRequest = HttpContext.GetOpenIddictServerRequest()
            ?? throw new InvalidOperationException("The OpenID Connect request cannot be retrieved.");

        var authenticateResult = await HttpContext.AuthenticateAsync(IdentityConstants.ApplicationScheme);

        if (!authenticateResult.Succeeded)
        {
            return Challenge(
                authenticationSchemes: IdentityConstants.ApplicationScheme,
                properties: new AuthenticationProperties
                {
                    RedirectUri = Request.PathBase + Request.Path + Request.QueryString
                }
            );
        }

        var command = new AuthorizeCommand(openIddictRequest, authenticateResult.Principal);
        var result = await _sender.Send(command, cancellationToken);

        if (result.IsFailed)
            throw new InvalidOperationException(result.Errors[0]?.Message);

        return SignIn(new ClaimsPrincipal(result.Value), OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
    }

    [HttpPost("connect/token")]
    public async Task<IActionResult> Exchange(CancellationToken cancellationToken)
    {
        var openIddictRequest = HttpContext.GetOpenIddictServerRequest() ??
            throw new InvalidOperationException("The OpenID Connect request cannot be retrieved.");

        if (openIddictRequest.IsAuthorizationCodeGrantType() || openIddictRequest.IsRefreshTokenGrantType())
        {
            var authenticateResult = await HttpContext.AuthenticateAsync(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);

            var user = await _userManager.FindByIdAsync(authenticateResult.Principal!.GetClaim(Claims.Subject)!);
            if (user is null)
            {
                return Forbid(
                    authenticationSchemes: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
                    properties: new AuthenticationProperties(new Dictionary<string, string?>
                    {
                        [OpenIddictServerAspNetCoreConstants.Properties.Error] = Errors.InvalidGrant,
                        [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = "The token is no longer valid."
                    }));
            }

            if (!await _signInManager.CanSignInAsync(user))
            {
                return Forbid(
                    authenticationSchemes: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
                    properties: new AuthenticationProperties(new Dictionary<string, string?>
                    {
                        [OpenIddictServerAspNetCoreConstants.Properties.Error] = Errors.InvalidGrant,
                        [OpenIddictServerAspNetCoreConstants.Properties.ErrorDescription] = "The user is no longer allowed to sign in."
                    }));
            }

            var request = new ExchangeCommand(user, authenticateResult.Principal!);
            var result = await _sender.Send(request, cancellationToken);

            return SignIn(new ClaimsPrincipal(result.Value), OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        }

        return BadRequest("The specified grant type is not supported.");
    }
}
