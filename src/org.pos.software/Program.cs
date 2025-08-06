using org.pos.software.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using org.pos.software.Infrastructure.Persistence.Repositories;
using org.pos.software.Application.Ports;
using org.pos.software.Application.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var configuration = builder.Configuration;

var bdConnectionString = configuration.GetValue<string>("CONNECTION_DEFAULT")
    ?? throw new InvalidOperationException("Error de conexion con la base de datos");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(bdConnectionString)
);

builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.EnsureCreated();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
