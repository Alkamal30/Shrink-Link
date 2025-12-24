using Gateway.Presentation;
using Gateway.Presentation.Middlewares;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAntiforgery(options =>
    {
        options.HeaderName = Constants.AntiforgeryHeaderName;
        options.Cookie.Name = Constants.AntiforgeryCookieName;
        options.Cookie.HttpOnly = true;
        options.Cookie.SameSite = SameSiteMode.Lax;
        options.Cookie.SecurePolicy = builder.Environment.IsDevelopment() ? CookieSecurePolicy.SameAsRequest : CookieSecurePolicy.Always;
    });

builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
    })
    .AddCookie(options =>
    {
        options.Cookie.Name = Constants.AuthenticationCookieName;
        options.Cookie.HttpOnly = true;
        options.Cookie.SameSite = SameSiteMode.Lax;
        options.Cookie.SecurePolicy = builder.Environment.IsDevelopment() ? CookieSecurePolicy.SameAsRequest : CookieSecurePolicy.Always;
        
        options.Events = new CookieAuthenticationEvents
        {
            OnRedirectToLogin = context =>
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return Task.CompletedTask;
            },
            OnRedirectToAccessDenied = context =>
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                return Task.CompletedTask;
            }
        };
    })
    .AddOpenIdConnect(options =>
    {
        options.Authority = builder.Configuration["Authentication:Authority"];
        options.RequireHttpsMetadata = builder.Environment.IsProduction();
        options.ClientId = builder.Configuration["Authentication:ClientId"];
        options.ClientSecret = builder.Configuration["Authentication:ClientSecret"];
        options.ResponseType = "code";
        options.UsePkce = true;
        options.SaveTokens = true;
        options.GetClaimsFromUserInfoEndpoint = true;

        options.Scope.Clear();
        options.Scope.Add("openid");
        options.Scope.Add("profile");
        options.Scope.Add("email");
        options.Scope.Add("api");
    });
builder.Services.AddAuthorization();

builder.Services.AddAuthorizationBuilder()
    .AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

// TODO: Create own ValidateAntiForgeryAttribute and remove views
builder.Services.AddControllersWithViews();

var app = builder.Build();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<AntiforgeryMiddleware>();

app.MapControllers();
app.MapReverseProxy();

app.MapWhen(
    x => !x.Request.Path.StartsWithSegments("/api")
        && !x.Request.Path.StartsWithSegments("/bff")
        && !x.Request.Path.StartsWithSegments("/auth")
        && !x.Request.Path.StartsWithSegments("/r"),
    builder =>
    {
        if (app.Environment.IsDevelopment())
        {
            builder.UseSpa(config => config.UseProxyToSpaDevelopmentServer(
                app.Configuration["FrontendUrl"] ?? throw new InvalidOperationException("FrontendUrl is null."))
            );
        }
        else
        {
            builder.UseStaticFiles();
            // add fallback
        }
    });

app.Run();