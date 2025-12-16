using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ShrinkLink.AuthService.Domain.Data;
using ShrinkLink.AuthService.Infrastructure.Data;
using ShrinkLink.AuthService.Infrastructure.Data.Entities;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AuthServiceDbContext>(options
    => options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));
builder.Services.AddScoped<IAuthServiceDbContext>(provider
    => provider.GetRequiredService<AuthServiceDbContext>());

builder.Services.AddIdentityCore<Identity>(o => o.User.RequireUniqueEmail = true)
    .AddRoles<IdentityRole<Guid>>()
    .AddEntityFrameworkStores<AuthServiceDbContext>();

builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
else
{
    app.UseHttpsRedirection();
}

app.Run();