using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using org.pos.software.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Configuración básica de servicios
builder.Services.AddControllers();

// Configuraciones modulares de servicios
SwaggerConfig.ConfigureServices(builder.Services);
DependencyInjection.ConfigureServices(builder.Services, builder.Configuration);
JwtConfig.ConfigureServices(builder.Services, builder.Configuration);
CorsConfig.ConfigureServices(builder.Services, builder.Configuration);

var app = builder.Build();

// Configuración del middleware
MiddlewareConfig.Configure(app, builder.Configuration);

app.MapGet("/", () =>
{

    var currentTime = DateTime.Now.ToString("HH:mm:ss");

    return Results.Ok(new
    {
        message = "¡API Funcionando!",
        time = currentTime,
        status = true
    });

}).RequireRateLimiting("fijo");

app.Run();

// necesario para los test de integracion
namespace org.pos.software
{
    public partial class Program { }
}