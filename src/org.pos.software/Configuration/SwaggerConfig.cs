using Microsoft.OpenApi.Models;

namespace org.pos.software.Configuration;

public static class SwaggerConfig
{
    public static void ConfigureServices(IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "POS Software API",
                Version = "v1",
                Description = "API para el software de punto de venta (POS)",
            });
        });
    }

    public static void ConfigureUI(WebApplication app)
    {
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
}