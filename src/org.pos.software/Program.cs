using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using org.pos.software.Application.Ports;
using org.pos.software.Application.Services;
using org.pos.software.Infrastructure.Persistence;
using org.pos.software.Infrastructure.Persistence.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
//builder.Services.AddOpenApi();

// Configuracion de Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "POS Software API",
        Version = "v1",
        Description = "API para el software de punto de venta (POS)",
    });
});

// Configuracion de la base de datos
var configuration = builder.Configuration;
var bdConnectionString = configuration.GetValue<string>("CONNECTION_DEFAULT")
    ?? throw new InvalidOperationException("Error de conexion con la base de datos");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(bdConnectionString)
);

// Inyeccion de dependencias
builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

var app = builder.Build();

// Migraciones y creacion de la base de datos
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.EnsureCreated();
}

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "POS Software API v1");
        c.DisplayOperationId();
        c.DisplayRequestDuration();
        c.EnableDeepLinking();
        c.EnableFilter();
        c.ShowExtensions();
    });
}


app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
