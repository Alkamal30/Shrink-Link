using Microsoft.EntityFrameworkCore;
using OpenTelemetry.Logs;
using OpenTelemetry.Resources;
using ShrinkLink.LinkService.Application.Common.Behaviors;
using ShrinkLink.LinkService.Domain.Data;
using ShrinkLink.LinkService.Domain.Services;
using ShrinkLink.LinkService.Infrastructure.Data;
using ShrinkLink.LinkService.Infrastructure.Services;
using ShrinkLink.LinkService.Infrastructure.Services.Grpc;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Services.AddOpenTelemetry()
    .ConfigureResource(r => r.AddService(builder.Environment.ApplicationName))
    .WithLogging(x => x.AddOtlpExporter(otlp =>
    {
        otlp.Endpoint = new Uri(builder.Configuration["Otlp:Endpoint"]!);
    }));

builder.Services.AddDbContext<LinkServiceContext>(options =>
        options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<ILinkServiceContext>(provider =>
    provider.GetRequiredService<LinkServiceContext>());
builder.Services.AddScoped<IShortCodeService, ShortCodeService>();
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
    cfg.AddOpenBehavior(typeof(LoggingBehavior<,>));
});
builder.Services.AddControllers();
builder.Services.AddGrpc();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// TODO: Find a better solution and remove this code
using var scope = app.Services.CreateScope();
var dbContext = scope.ServiceProvider.GetRequiredService<LinkServiceContext>();
if (dbContext.Database.GetPendingMigrations().Any())
{
    await dbContext.Database.MigrateAsync();
}

app.MapControllers();
app.MapGrpcService<LinkGrpcService>();

app.Run();
