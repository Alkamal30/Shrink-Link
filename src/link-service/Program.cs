using Microsoft.EntityFrameworkCore;
using ShrinkLink.LinkService.Domain.Data;
using ShrinkLink.LinkService.Domain.Services;
using ShrinkLink.LinkService.Infrastructure.Data;
using ShrinkLink.LinkService.Infrastructure.Services;
using ShrinkLink.LinkService.Infrastructure.Services.Grpc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<LinkServiceContext>(options =>
		options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<ILinkServiceContext>(provider =>
    provider.GetRequiredService<LinkServiceContext>());
builder.Services.AddScoped<IShortCodeService, ShortCodeService>();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));
builder.Services.AddControllers();
builder.Services.AddGrpc();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    // TODO: Find a better solution and remove this code
    using var scope = app.Services.CreateScope();
    var dbContext =  scope.ServiceProvider.GetRequiredService<LinkServiceContext>();
    if (dbContext.Database.GetPendingMigrations().Any())
    {
        await  dbContext.Database.MigrateAsync();    
    }
    
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.MapGrpcService<LinkGrpcService>();

app.Run();
