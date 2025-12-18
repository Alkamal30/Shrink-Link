using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ShrinkLink.AuthService.Application.Helpers;
using ShrinkLink.AuthService.Domain.Data;
using ShrinkLink.AuthService.Infrastructure.Data;
using ShrinkLink.AuthService.Infrastructure.Data.Entities;
using static OpenIddict.Abstractions.OpenIddictConstants;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AuthServiceDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("Default"));
    options.UseOpenIddict();
});
builder.Services.AddScoped<IAuthServiceDbContext>(provider
    => provider.GetRequiredService<AuthServiceDbContext>());

builder.Services.AddIdentityCore<Identity>(o => o.User.RequireUniqueEmail = true)
    .AddRoles<IdentityRole<Guid>>()
    .AddEntityFrameworkStores<AuthServiceDbContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication(IdentityConstants.ApplicationScheme)
    .AddCookie(IdentityConstants.ApplicationScheme, options =>
    {
        options.LoginPath =  "/api/auth/login";
        options.LogoutPath = "/api/auth/logout";
    });
builder.Services.AddAuthorization();

builder.Services.AddOpenIddict()
    .AddCore(options =>
    {
        options.UseEntityFrameworkCore()
            .UseDbContext<AuthServiceDbContext>();
    })
    .AddServer(options =>
    {
        options.SetIssuer(
            new Uri(
                builder.Configuration["Authentication:Issuer"]
                    ?? throw new NullReferenceException("Authentication:Issuer cannot be null!")
            )
        );

        options.SetAuthorizationEndpointUris("connect/authorize")
            .SetTokenEndpointUris("connect/token")
            .AllowAuthorizationCodeFlow()
            .AllowRefreshTokenFlow()
            .RequireProofKeyForCodeExchange()
            .RegisterScopes(
                Scopes.OpenId,
                Scopes.Profile,
                Scopes.Email,
                "api"
            );

        var aspNetCoreBuilder = options.UseAspNetCore()
            .EnableAuthorizationEndpointPassthrough()
            .EnableTokenEndpointPassthrough();

        if (builder.Environment.IsDevelopment())
        {
            options.AddDevelopmentEncryptionCertificate()
               .AddDevelopmentSigningCertificate();

            aspNetCoreBuilder.DisableTransportSecurityRequirement();
        }
        else
        {
            // TODO: Add certificate for production
        }
    });

builder.Services.AddControllers();
builder.Services.AddOpenApi();

// TODO: Check if it is safe
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders =
        ForwardedHeaders.XForwardedFor |
        ForwardedHeaders.XForwardedProto |
        ForwardedHeaders.XForwardedHost;
});

var app = builder.Build();

app.UseForwardedHeaders();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "OpenAPI v1");
    });
}

app.MapControllers();

OpenIddictApplicationsRegistrator.RegisterApplications(app);

app.Run();