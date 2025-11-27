using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ShrinkLink.UserService.Application.Abstractions;
using ShrinkLink.UserService.Domain.Data;
using ShrinkLink.UserService.Domain.Entities;
using ShrinkLink.UserService.Infrastructure.Data;
using ShrinkLink.UserService.Infrastructure.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddDbContext<UserServiceContext>(options =>
        options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")))
    .AddScoped<IUserServiceContext>(provider => provider.GetRequiredService<UserServiceContext>())
    .AddScoped<IPasswordHasher<User>, PasswordHasher<User>>()
    .AddScoped<IJwtTokenGenerator, JwtTokenGenerator>()
    .AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:SecretKey"]!)),
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero,
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["JwtSettings:Issuer"]!,
            ValidateAudience = true,
            ValidAudience = builder.Configuration["JwtSettings:Audience"]!,
        };
    });

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    // TODO: Find a better solution and remove this code
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<UserServiceContext>();
    if (dbContext.Database.GetPendingMigrations().Any())
    {
        await dbContext.Database.MigrateAsync();
    }

    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();