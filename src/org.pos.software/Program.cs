using org.pos.software.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Configuración básica de servicios
builder.Services.AddControllers();

// Configuraciones modulares de servicios
SwaggerConfig.ConfigureServices(builder.Services);
DependencyInjection.ConfigureServices(builder.Services, builder.Configuration);

var app = builder.Build();

// Configuración del middleware
MiddlewareConfig.Configure(app, builder.Configuration);

app.Run();