using Microsoft.EntityFrameworkCore;
using MediatR;
using ShrinkLink.LinkService.Domain.Data;
using ShrinkLink.LinkService.Domain.Services;
using ShrinkLink.LinkService.Infrastructure.Data;
using ShrinkLink.LinkService.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<LinkServiceContext>(options =>
		options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<ILinkServiceContext>(provider =>
    provider.GetService<LinkServiceContext>());
builder.Services.AddScoped<IShortCodeService, ShortCodeService>();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
