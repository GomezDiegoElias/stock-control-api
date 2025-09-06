using FluentValidation;
using Microsoft.AspNetCore.RateLimiting;
using org.pos.software.Application.InPort;
using org.pos.software.Application.Ports;
using org.pos.software.Application.Services;
using org.pos.software.Domain.OutPort;
using org.pos.software.Infrastructure.Persistence.SqlServer.Repositories;
using org.pos.software.Utils.Validations;

namespace org.pos.software.Configuration;

public static class DependencyInjection
{
    public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        ConfigureDatabase(services, configuration);
        ConfigureApplicationServices(services);
    }

    // repositorios y contexto de base de datos
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
                services.AddScoped<IUserRepository, UserRepository>();
                services.AddScoped<IRoleRepository, RoleRepository>();
                services.AddScoped<IClientRepository, ClientRepository>();
                services.AddScoped<IEmployeeRepository, EmployeeRepository>();
                break;
            default:
                throw new InvalidOperationException($"Proveedor de base de datos no soportado: {dbProvider}");
        }
    }

    private static void ConfigureApplicationServices(IServiceCollection services)
    {
        // Limitar peticiones
        services.AddRateLimiter(options =>
        {
            options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
            options.AddFixedWindowLimiter("fijo", opt =>
            {
                opt.PermitLimit = 2;
                opt.Window = TimeSpan.FromSeconds(5);
            });
        });

        services.AddAuthorization(options =>
        {

            var entities = new[] { "User", "Client" };
            var actions = new[] { "CREATE", "READ", "UPDATE", "DELETE" };

            foreach (var entity in entities)
            {
                foreach (var action in actions)
                {

                    var permission = $"{action}_{entity}".ToUpper(); // ej: CREATE_CLIENT
                    var policyName = $"Can{action}_{entity}"; // ej: CanCREATE_Client
                
                    options.AddPolicy(policyName, policy => policy.RequireClaim("permission", permission) );

                }
            }

        });

        // validadores
        services.AddValidatorsFromAssemblyContaining<LoginValidation>();
        services.AddValidatorsFromAssemblyContaining<RegisterValidation>();

        // Casos de uso y servicios
        services.AddScoped<IRoleService, RoleService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IClientService, ClientService>();
        services.AddScoped<IEmployeeService, EmployeeService>();

    }
}