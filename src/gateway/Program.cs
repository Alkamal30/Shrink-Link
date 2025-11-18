var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy
                .WithOrigins(builder.Configuration.GetValue<string>("FrontendUrl") ?? string.Empty)
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

var app = builder.Build();

app.UseCors("AllowFrontend");
app.MapReverseProxy();

app.Run();