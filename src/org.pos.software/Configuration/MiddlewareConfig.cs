namespace org.pos.software.Configuration;

public static class MiddlewareConfig
{
    public static void Configure(WebApplication app, IConfiguration configuration)
    {
        // Database initialization
        DatabaseConfig.EnsureDatabaseCreated(app, configuration);

        // Development-specific configurations
        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            // SwaggerConfig.ConfigureUI(app);
        }

        // Swagger en TODOS los entornos (Desarrollo, Pruebas, Producción)
        SwaggerConfig.ConfigureUI(app);

        // Common middleware
        app.UseRateLimiter();
        app.UseHttpsRedirection();
        app.UseCors();
        app.UseAuthentication(); // jwt
        app.UseAuthorization();
        app.MapControllers().RequireRateLimiting("fijo");
    }
}