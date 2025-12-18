using OpenIddict.Abstractions;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace ShrinkLink.AuthService.Application.Helpers;

public static class OpenIddictApplicationsRegistrator
{
    private const string SpaClientId = "spa";

    public static void RegisterApplications(WebApplication app)
    {
        RegisterSpaClientApplication(app);
    }

    private static void RegisterSpaClientApplication(WebApplication app)
    {
        app.Lifetime.ApplicationStarted.Register(async () =>
        {
            using var scope = app.Services.CreateScope();
            var manager = scope.ServiceProvider.GetRequiredService<IOpenIddictApplicationManager>();

            if (await manager.FindByClientIdAsync(SpaClientId, CancellationToken.None) is null)
            {
                await manager.CreateAsync(new OpenIddictApplicationDescriptor
                {
                    ClientId = SpaClientId,
                    ClientType = ClientTypes.Public,
                    DisplayName = "Web SPA",
                    ApplicationType = ApplicationTypes.Web,
                    RedirectUris =
                    {
                        new Uri("http://localhost:5173/auth/callback")
                    },
                    PostLogoutRedirectUris =
                    {
                        new Uri("http://localhost:5173/")
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
