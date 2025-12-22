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
        return Challenge(
            authenticationSchemes: OpenIdConnectDefaults.AuthenticationScheme,
            properties: new AuthenticationProperties
            {
                RedirectUri = redirectUrl ?? "/",
            }
        );
    }

    [HttpGet(nameof(Me))]
    public async Task<IActionResult> Me()
    {
        return Ok(new
        {
            isAuthenticated = User.Identity?.IsAuthenticated ?? false,
            name = User.Identity?.Name,
            subject = User.FindFirst(ClaimTypes.NameIdentifier)?.Value,
            claims = User.Claims.Select(c => new { c.Type, c.Value }),
        });
    }

    [HttpPost(nameof(LogOut))]
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
