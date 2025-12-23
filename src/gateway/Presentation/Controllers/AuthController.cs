using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Gateway.Presentation.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    [HttpGet(nameof(SignIn))]
    public async Task<IActionResult> SignIn([FromQuery] string? redirectUrl)
    {
        if (!Url.IsLocalUrl(redirectUrl))
            redirectUrl = "/";

        return Challenge(
            authenticationSchemes: OpenIdConnectDefaults.AuthenticationScheme,
            properties: new AuthenticationProperties
            {
                RedirectUri = redirectUrl,
            }
        );
    }

    [HttpGet(nameof(Me))]
    public async Task<IActionResult> Me()
    {
        if (!(User.Identity?.IsAuthenticated ?? false))
            return Ok(new { isAuthenticated = false });


        return Ok(new
        {
            isAuthenticated = true,
            sub = User.FindFirstValue(ClaimTypes.NameIdentifier),
            name = User.FindFirstValue("name"),
            email = User.FindFirstValue(ClaimTypes.Email),
            roles = User.FindAll(ClaimTypes.Role).Select(x => x.Value).Distinct(),
        });
    }

    [HttpPost(nameof(LogOut))]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> LogOut()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        await HttpContext.SignOutAsync(OpenIdConnectDefaults.AuthenticationScheme, new AuthenticationProperties
        {
            RedirectUri = "/"
        });

        return NoContent();
    }
}
