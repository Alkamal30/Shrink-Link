using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Mvc;

namespace Gateway.Presentation.Controllers;

[ApiController]
[Route("[controller]")]
public class BffController(IAntiforgery antiforgery) : ControllerBase
{
    private readonly IAntiforgery _antiforgery = antiforgery;

    [HttpGet("csrf")]
    public async Task<IActionResult> GetCsrfToken()
    {
        var tokens = _antiforgery.GetAndStoreTokens(HttpContext);

        return Ok(new { token = tokens.RequestToken });
    }
}
