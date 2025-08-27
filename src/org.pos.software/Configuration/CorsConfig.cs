namespace org.pos.software.Configuration
{
    public static class CorsConfig
    {

        public static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            var allowedOrigins = configuration.GetSection("Cors:AllowedOrigins").Get<string[]>();

            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    if (allowedOrigins != null && allowedOrigins.Length > 0)
                    {
                        policy.WithOrigins(allowedOrigins)
                              .AllowAnyHeader()
                              .AllowAnyMethod();
                    }
                    else
                    {
                        policy.AllowAnyOrigin()
                              .AllowAnyHeader()
                              .AllowAnyMethod();
                    }
                });
            });
        }
    }
}
