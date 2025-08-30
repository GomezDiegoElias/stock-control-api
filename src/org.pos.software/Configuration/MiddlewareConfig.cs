using System.Threading.Tasks;
using org.pos.software.Infrastructure.Persistence.MySql;
using org.pos.software.Infrastructure.Persistence.SqlServer;
using org.pos.software.Utils.Seeder;

namespace org.pos.software.Configuration;

public static class MiddlewareConfig
{
    public static async Task Configure(WebApplication app, IConfiguration configuration)
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

        using var scope = app.Services.CreateScope();

        //var db = scope.ServiceProvider.GetRequiredService<MySqlDbContext>();
        //await MySqlDbSedder.SeedRolesAndPermissions(db);
        //await MySqlDbSedder.SeedUserWhitDiferentRoles(db);

        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        await DbSedder.SeedRolesAndPermissions(db);
        await DbSedder.SeedUserWhitDiferentRoles(db);

    }
}