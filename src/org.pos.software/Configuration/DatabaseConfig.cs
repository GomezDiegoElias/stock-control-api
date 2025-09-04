using Microsoft.EntityFrameworkCore;
using org.pos.software.Infrastructure.Persistence.SqlServer;

namespace org.pos.software.Configuration;

public static class DatabaseConfig
{
    public static DbContext GetDbContext(IServiceProvider serviceProvider, IConfiguration configuration)
    {
        var dbProvider = configuration.GetValue<string>("Database:Provider")?.Trim().ToLowerInvariant();

        return dbProvider switch
        {
            "sqlserver" => serviceProvider.GetRequiredService<AppDbContext>(),
            _ => throw new InvalidOperationException($"Proveedor de base de datos no soportado: {dbProvider}")
        };
    }

    public static void EnsureDatabaseCreated(WebApplication app, IConfiguration configuration)
    {
        using var scope = app.Services.CreateScope();
        var dbContext = GetDbContext(scope.ServiceProvider, configuration);
        dbContext.Database.EnsureCreated();
    }

    internal static void ConfigureSqlServer(IServiceCollection services, string connectionString)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(connectionString));
    }

}