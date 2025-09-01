using System.Threading.Tasks;
using Microsoft.AspNetCore.Diagnostics;
using org.pos.software.Domain.Exceptions;
using org.pos.software.Infrastructure.Persistence.MySql;
using org.pos.software.Infrastructure.Persistence.SqlServer;
using org.pos.software.Infrastructure.Rest.Dto.Response.General;
using org.pos.software.Utils.Seeder;

namespace org.pos.software.Configuration;

public static class MiddlewareConfig
{
    public static async Task Configure(WebApplication app, IConfiguration configuration)
    {

        // Manejo global de excepciones
        app.UseExceptionHandler(appError =>
        {
            appError.Run(async context =>
            {
                context.Response.ContentType = "application/json";

                var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                if (contextFeature != null)
                {
                    var ex = contextFeature.Error;
                    int statusCode = ex switch
                    {
                        // Excepciones comunes
                        KeyNotFoundException => 404,
                        UnauthorizedAccessException => 401,
                        ApplicationException => 400,
                        // Excepciones personalizas
                        UserNotFoundException => 404,
                        _ => 500 // Internal Server
                    };

                    context.Response.StatusCode = statusCode;

                    var error = new ErrorDetails(statusCode, ex.Message, context.Request.Path, null); // ex.StackTrace

                    var response = new StandardResponse<string>(false, "Something went wrong", null, error, statusCode);

                    await context.Response.WriteAsJsonAsync(response);

                }
            });
        });

        // Capturar error de mal URL
        app.Use(async (context, next) =>
        {
            await next();

            if (context.Response.StatusCode == StatusCodes.Status404NotFound &&
                !context.Response.HasStarted)
            {
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsJsonAsync(new
                {
                    StatusCode = 404,
                    Message = $"The endpoint {context.Request.Path} was not found."
                });
            }
        });

        // Database initialization
        DatabaseConfig.EnsureDatabaseCreated(app, configuration);

        // Development-specific configurations
        //if (app.Environment.IsDevelopment())
        //{
        //    app.UseDeveloperExceptionPage();
        //    // SwaggerConfig.ConfigureUI(app);
        //}

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
        await DbSedder.SeedStoredProceduresPaginationUser(db);

    }
}