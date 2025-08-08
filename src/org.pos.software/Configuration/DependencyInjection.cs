using org.pos.software.Application.Ports;
using org.pos.software.Application.Services;
using org.pos.software.Infrastructure.Persistence.SqlServer.Repositories;
using org.pos.software.Infrastructure.Persistence.Supabase.Repositories;
using org.pos.software.Infrastructure.Persistence.MySql.Repositories;

namespace org.pos.software.Configuration;

public static class DependencyInjection
{
    public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        ConfigureDatabase(services, configuration);
        ConfigureApplicationServices(services);
    }

    private static void ConfigureDatabase(IServiceCollection services, IConfiguration configuration)
    {
        var dbProvider = configuration.GetValue<string>("Database:Provider")?.Trim();

        if (string.IsNullOrEmpty(dbProvider))
        {
            throw new InvalidOperationException("No se ha configurado un proveedor de base de datos válido");
        }

        var connectionString = configuration.GetValue<string>($"CONNECTION_{dbProvider.ToUpper()}")
            ?? throw new InvalidOperationException($"Error al obtener la cadena de conexión para {dbProvider}");

        switch (dbProvider.ToLowerInvariant())
        {
            case "sqlserver":
                DatabaseConfig.ConfigureSqlServer(services, connectionString);
                services.AddScoped<UserRepository>();
                break;
            case "supabase":
                DatabaseConfig.ConfigureSupabase(services, connectionString);
                services.AddScoped<SupabaseUserRepository>();
                break;
            case "mysql":
                DatabaseConfig.ConfigureMySql(services, connectionString);
                services.AddScoped<MySqlUserRepository>();
                break;
            default:
                throw new InvalidOperationException($"Proveedor de base de datos no soportado: {dbProvider}");
        }
    }

    private static void ConfigureApplicationServices(IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        // Registrar otros servicios de aplicación aquí
    }
}