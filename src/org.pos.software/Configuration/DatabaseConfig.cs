using Microsoft.EntityFrameworkCore;
using org.pos.software.Infrastructure.Persistence.SqlServer;
using org.pos.software.Infrastructure.Persistence.Supabase;
using org.pos.software.Infrastructure.Persistence.MySql;

namespace org.pos.software.Configuration;

public static class DatabaseConfig
{
    public static DbContext GetDbContext(IServiceProvider serviceProvider, IConfiguration configuration)
    {
        var dbProvider = configuration.GetValue<string>("Database:Provider")?.Trim().ToLowerInvariant();

        return dbProvider switch
        {
            "sqlserver" => serviceProvider.GetRequiredService<AppDbContext>(),
            "supabase" => serviceProvider.GetRequiredService<SupabaseDbContext>(),
            "mysql" => serviceProvider.GetRequiredService<MySqlDbContext>(),
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

    internal static void ConfigureSupabase(IServiceCollection services, string connectionString)
    {
        services.AddDbContext<SupabaseDbContext>(options =>
            options.UseNpgsql(connectionString, npgsqlOptions =>
            {
                npgsqlOptions.CommandTimeout(120);
            }));
    }

    internal static void ConfigureMySql(IServiceCollection services, string connectionString)
    {
        services.AddDbContext<MySqlDbContext>(options =>
            options.UseMySQL(connectionString));
    }
}