using Microsoft.AspNetCore.Antiforgery;

namespace Gateway.Presentation.Middlewares;

public class AntiforgeryMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;

    public async Task InvokeAsync(HttpContext context, IAntiforgery antiforgery)
    {
        var requestPath = context.Request.Path.Value;

        if (string.Equals(requestPath, "/", StringComparison.OrdinalIgnoreCase)
            || string.Equals(requestPath, "/index.html", StringComparison.OrdinalIgnoreCase))
        {
            var tokenSet = antiforgery.GetAndStoreTokens(context);
            context.Response.Cookies.Append(Constants.AntiforgeryHeaderName, tokenSet.RequestToken!,
                new CookieOptions { HttpOnly = false });
        }

        await _next(context);
    }
}
