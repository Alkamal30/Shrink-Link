using OpenIddict.Abstractions;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace ShrinkLink.AuthService.Application.Helpers;

public static class OpenIddictApplicationsRegistrator
{
    public static void RegisterApplications(WebApplication app)
    {
        RegisterWebBffClientApplication(app);
    }

    private static void RegisterWebBffClientApplication(WebApplication app)
    {
        app.Lifetime.ApplicationStarted.Register(async () =>
        {
            using var scope = app.Services.CreateScope();
            var manager = scope.ServiceProvider.GetRequiredService<IOpenIddictApplicationManager>();
            var clientId = app.Configuration["Authentication:WebBffClientId"]
                ?? throw new InvalidOperationException("Web BFF Client Id is not defined.");

            if (await manager.FindByClientIdAsync(clientId, CancellationToken.None) is null)
            {
                await manager.CreateAsync(new OpenIddictApplicationDescriptor
                {
                    ClientId = clientId,
                    ClientSecret = app.Configuration["Authentication:WebBffClientSecret"],
                    ClientType = ClientTypes.Confidential,
                    DisplayName = "Web BFF",
                    ApplicationType = ApplicationTypes.Web,
                    RedirectUris =
                    {
                        new Uri("http://localhost:5001/signin-oidc")
                    },
                    PostLogoutRedirectUris =
                    {
                        new Uri("http://localhost:5001/")
                    },
                    Permissions =
                    {
                        Permissions.Endpoints.Authorization,
                        Permissions.Endpoints.Token,

                        Permissions.GrantTypes.AuthorizationCode,
                        Permissions.GrantTypes.RefreshToken,
                        Permissions.ResponseTypes.Code,

                        Permissions.Scopes.Profile,
                        Permissions.Scopes.Email,
                        Permissions.Prefixes.Scope + "api"
                    },
                    Requirements =
                    {
                        Requirements.Features.ProofKeyForCodeExchange
                    }
                }, CancellationToken.None);
            }
        });
    }
}
