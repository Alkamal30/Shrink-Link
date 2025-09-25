using Microsoft.EntityFrameworkCore;
using MediatR;
using ShrinkLink.LinkService;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<LinkServiceContext>(options =>
		options.UseSqlite("Data Source=database.db"));
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
