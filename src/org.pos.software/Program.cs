using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using org.pos.software.Application.Ports;
using org.pos.software.Application.Services;
using org.pos.software.Infrastructure.Persistence.SqlServer;
using org.pos.software.Infrastructure.Persistence.Supabase;
using org.pos.software.Infrastructure.Persistence.SqlServer.Repositories;
using org.pos.software.Infrastructure.Persistence.Supabase.Repositories;
using org.pos.software.Infrastructure.Persistence.MySql.Repositories;
using org.pos.software.Infrastructure.Persistence.MySql;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
//builder.Services.AddOpenApi();

// Configuracion de Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "POS Software API",
        Version = "v1",
        Description = "API para el software de punto de venta (POS)",
    });
});

// Configuracion de la base de datos
var configuration = builder.Configuration;

var dbProvider = configuration.GetValue<string>("Database:Provider"); // SqlServer o Supabase

if (dbProvider == "SqlServer")
{

    var bdConnectionString = configuration.GetValue<string>("CONNECTION_SQLSERVER")
        ?? throw new InvalidOperationException("Error al obtener la cadena de conexion");

    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseSqlServer(bdConnectionString)
    );

    // Inyeccion de dependencias
    builder.Services.AddScoped<UserRepository>();

} else if (dbProvider == "Supabase")
{
 
    //builder.Services.AddDbContext<SupabaseDbContext>(options =>
    //    options.UseNpgsql(configuration.GetConnectionString("CONNECTION_SUPABASE"))
    //);

    var bdConnectionString = configuration.GetValue<string>("CONNECTION_SUPABASE")
        ?? throw new InvalidOperationException("Error al obtener la cadena de conexion");

    //builder.Services.AddDbContext<SupabaseDbContext>(options =>
    //    options.UseNpgsql(bdConnectionString)
    //);

    //bdConnectionString += ";Pooling=true;Minimum Pool Size=1;Maximum Pool Size=20;Ssl Mode=Require;Trust Server Certificate=true";

    builder.Services.AddDbContext<SupabaseDbContext>(options =>
        options.UseNpgsql(bdConnectionString, npgsqlOptions =>
        {
            npgsqlOptions.CommandTimeout(120); // 2 minutos
        })
    );


    // Inyeccion de dependencias
    builder.Services.AddScoped<SupabaseUserRepository>();

} else if (dbProvider == "MySql")
{

    var bdConnectionString = configuration.GetValue<string>("CONNECTION_MYSQL")
        ?? throw new InvalidOperationException("Error al obtener la cadena de conexion");

    builder.Services.AddDbContext<MySqlDbContext>(options =>
        options.UseMySQL(bdConnectionString)
    );

    // Inyeccion de dependencias
    builder.Services.AddScoped<MySqlUserRepository>();

}

    builder.Services.AddScoped<IUserService, UserService>();


var app = builder.Build(); //////////////////////////////////////////////////////// VAR APP = BUILDER.BUILD();

// Migraciones y creacion de la base de datos
//using (var scope = app.Services.CreateScope())
//{
//    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
//    dbContext.Database.EnsureCreated();
//}

using (var scope = app.Services.CreateScope())
{
    if (dbProvider == "SqlServer")
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        dbContext.Database.EnsureCreated();
    }
    else if (dbProvider == "Supabase")
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<SupabaseDbContext>();
        dbContext.Database.EnsureCreated();
    } else if (dbProvider == "MySql")
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<MySqlDbContext>();
        dbContext.Database.EnsureCreated();
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
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


app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
